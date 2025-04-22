using AxonPDS.DTOs.User;
using AxonPDS.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AxonPDS.Controller
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController(IUser userRepo) : ControllerBase
    {
         private readonly IUser _userRepo = userRepo;

        // Get All Users
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepo.GetAllUsersAsync();
            return Ok(users);
        }

        // Created New User
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            var user = await _userRepo.CreateUserAsync(userDto);

            if (user == null)
            {
                return BadRequest("User creation failed");
            }

            return Ok(user);
        }
    }
}
