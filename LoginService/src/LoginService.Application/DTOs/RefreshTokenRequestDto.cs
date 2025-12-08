using System.ComponentModel.DataAnnotations;

namespace LoginService.Application.DTOs;

public class RefreshTokenRequestDto
{
    [Required(ErrorMessage = "El refresh token es requerido")]
    public string RefreshToken { get; set; } = string.Empty;
}
