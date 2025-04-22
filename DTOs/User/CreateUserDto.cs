namespace AxonPDS.DTOs.User;

public record class CreateUserDto
{
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;    

    public string Password { get; set; } = string.Empty;

}
