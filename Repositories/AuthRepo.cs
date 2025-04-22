using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AxonPDS.DTOs.Auth;
using AxonPDS.Entities;
using AxonPDS.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace AxonPDS.Repositories;

public class AuthRepo(UserManager<User> userManager, IConfiguration configuration) : IAuth
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IConfiguration _configuration = configuration;

    public async Task<User?> LoginAsync(LoginDto body)
    {
        var user = await _userManager.FindByEmailAsync(body.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, body.Password))
        {
            return null;
        }

        return user;
    }

    public Task<string?> GenerateToken(User user)
    {
        var jwtSecret = _configuration["AppSettings:JWTSecret"];
        if (string.IsNullOrEmpty(jwtSecret))
        {
            return Task.FromResult<string?>(null);
        }

        var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
            ]),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha512Signature),
            Issuer = _configuration["AppSettings:Issuer"],    // Add this
            Audience = _configuration["AppSettings:Audience"] // Add this
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        
        return Task.FromResult<string?>(tokenHandler.WriteToken(securityToken));
    }
}