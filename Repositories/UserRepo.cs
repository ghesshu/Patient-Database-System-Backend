using AxonPDS.Data;
using AxonPDS.DTOs.User;
using AxonPDS.Entities;
using AxonPDS.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AxonPDS.Repositories;

public class UserRepo(UserManager<User> userManager, PdsDbContext appDBContext ) : IUser
{
    private readonly UserManager<User> _userManager = userManager;

    private readonly PdsDbContext service = appDBContext;

    public async Task<User?> CreateUserAsync(CreateUserDto userDto)
    {
        var user = new User
        {
            UserName = userDto.Email,
            Email = userDto.Email, 
            FullName = userDto.FullName
        };

        var result = await _userManager.CreateAsync(user, userDto.Password);

        if (!result.Succeeded)
        {
            // throw new ApplicationException("User creation failed");
            return null;
        }

        return user;
    }

     // 👇 Implement GetAllUsersAsync
    public async Task<List<User>> GetAllUsersAsync()
    {
        return await service.Users.ToListAsync();
    }
}
