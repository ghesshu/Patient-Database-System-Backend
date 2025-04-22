using AxonPDS.DTOs.Auth;
using AxonPDS.Entities;

namespace AxonPDS.Interfaces;

public interface IAuth
{
    Task<User?> LoginAsync(LoginDto loginDto);

    Task<string?> GenerateToken(User user);
}
