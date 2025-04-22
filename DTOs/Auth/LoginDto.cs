using System.ComponentModel.DataAnnotations;

namespace AxonPDS.DTOs.Auth;

public record class LoginDto
{
    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

}
