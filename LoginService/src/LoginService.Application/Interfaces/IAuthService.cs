using LoginService.Application.DTOs;

namespace LoginService.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequest, string ipAddress, string userAgent);
    Task<UserDto> RegisterAsync(RegisterRequestDto registerRequest);
    Task<LoginResponseDto> RefreshTokenAsync(string refreshToken, string ipAddress);
    Task<bool> RevokeTokenAsync(string refreshToken, string ipAddress);
    Task LogoutAsync(int userId, string ipAddress);
}
