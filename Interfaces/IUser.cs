using System;
using AxonPDS.Entities;
using AxonPDS.DTOs;
using AxonPDS.DTOs.User;


namespace AxonPDS.Interfaces;

public interface IUser
{
    Task<List<User>> GetAllUsersAsync();
    
    Task<User?> CreateUserAsync(CreateUserDto userDto);

}
