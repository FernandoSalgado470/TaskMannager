using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LoginService.Application.DTOs;
using LoginService.Application.Interfaces;
using LoginService.Domain.Entities;
using LoginService.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LoginService.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ILoginAttemptRepository _loginAttemptRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IConfiguration _configuration;

    public AuthService(
        IUserRepository userRepository,
        ILoginAttemptRepository loginAttemptRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _loginAttemptRepository = loginAttemptRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequest, string ipAddress, string userAgent)
    {
        // Buscar usuario por email
        var user = await _userRepository.GetByEmailAsync(loginRequest.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
        {
            // Registrar intento fallido
            if (user != null)
            {
                await _loginAttemptRepository.CreateAsync(new LoginAttempt
                {
                    UserId = user.Id,
                    IsSuccessful = false,
                    IpAddress = ipAddress,
                    UserAgent = userAgent,
                    FailureReason = "Contraseña incorrecta"
                });
            }

            throw new UnauthorizedAccessException("Credenciales inválidas");
        }

        if (!user.IsEnabled)
        {
            throw new UnauthorizedAccessException("Usuario deshabilitado");
        }

        // Verificar intentos fallidos recientes
        var failedAttempts = await _loginAttemptRepository.GetFailedAttemptsCountAsync(user.Id, TimeSpan.FromMinutes(15));
        if (failedAttempts >= 5)
        {
            throw new UnauthorizedAccessException("Demasiados intentos fallidos. Intenta más tarde.");
        }

        // Registrar intento exitoso
        await _loginAttemptRepository.CreateAsync(new LoginAttempt
        {
            UserId = user.Id,
            IsSuccessful = true,
            IpAddress = ipAddress,
            UserAgent = userAgent
        });

        // Actualizar último login
        user.LastLoginAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

        // Generar tokens
        var token = GenerateJwtToken(user);
        var refreshToken = await GenerateRefreshTokenAsync(user.Id, ipAddress);

        return new LoginResponseDto
        {
            Token = token,
            RefreshToken = refreshToken.Token,
            ExpiresAt = DateTime.UtcNow.AddHours(24),
            User = MapToUserDto(user)
        };
    }

    public async Task<UserDto> RegisterAsync(RegisterRequestDto registerRequest)
    {
        // Validar email único
        if (await _userRepository.EmailExistsAsync(registerRequest.Email))
        {
            throw new InvalidOperationException("El email ya está registrado");
        }

        // Validar username único
        if (await _userRepository.UsernameExistsAsync(registerRequest.Username))
        {
            throw new InvalidOperationException("El nombre de usuario ya está en uso");
        }

        // Crear usuario
        var user = new User
        {
            Username = registerRequest.Username,
            Email = registerRequest.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password),
            FullName = registerRequest.FullName,
            IsEnabled = true,
            CreatedAt = DateTime.UtcNow
        };

        var createdUser = await _userRepository.CreateAsync(user);
        return MapToUserDto(createdUser);
    }

    public async Task<LoginResponseDto> RefreshTokenAsync(string refreshToken, string ipAddress)
    {
        var token = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

        if (token == null || !token.IsActive)
        {
            throw new UnauthorizedAccessException("Refresh token inválido o expirado");
        }

        var user = await _userRepository.GetByIdAsync(token.UserId);
        if (user == null || !user.IsEnabled)
        {
            throw new UnauthorizedAccessException("Usuario no encontrado o deshabilitado");
        }

        // Revocar token antiguo
        token.RevokedAt = DateTime.UtcNow;
        token.IsRevoked = true;
        token.RevokedByIp = ipAddress;
        await _refreshTokenRepository.UpdateAsync(token);

        // Generar nuevos tokens
        var newJwtToken = GenerateJwtToken(user);
        var newRefreshToken = await GenerateRefreshTokenAsync(user.Id, ipAddress);

        return new LoginResponseDto
        {
            Token = newJwtToken,
            RefreshToken = newRefreshToken.Token,
            ExpiresAt = DateTime.UtcNow.AddHours(24),
            User = MapToUserDto(user)
        };
    }

    public async Task<bool> RevokeTokenAsync(string refreshToken, string ipAddress)
    {
        var token = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

        if (token == null || !token.IsActive)
        {
            return false;
        }

        token.RevokedAt = DateTime.UtcNow;
        token.IsRevoked = true;
        token.RevokedByIp = ipAddress;
        await _refreshTokenRepository.UpdateAsync(token);

        return true;
    }

    public async Task LogoutAsync(int userId, string ipAddress)
    {
        await _refreshTokenRepository.RevokeAllUserTokensAsync(userId, ipAddress);
    }

    private string GenerateJwtToken(User user)
    {
        var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key no configurado");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("username", user.Username),
            new Claim("fullName", user.FullName)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<RefreshToken> GenerateRefreshTokenAsync(int userId, string ipAddress)
    {
        var refreshToken = new RefreshToken
        {
            UserId = userId,
            Token = GenerateRandomToken(),
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow,
            CreatedByIp = ipAddress
        };

        return await _refreshTokenRepository.CreateAsync(refreshToken);
    }

    private string GenerateRandomToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    private UserDto MapToUserDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FullName = user.FullName,
            IsEnabled = user.IsEnabled,
            CreatedAt = user.CreatedAt,
            LastLoginAt = user.LastLoginAt
        };
    }
}
