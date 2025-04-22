using AxonPDS.DTOs.Auth;
using AxonPDS.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AxonPDS.Controller
{
    [Route("api/auth")]
    [ApiController]

    public class AuthController(IAuth authRepo) : ControllerBase
    {
        private readonly IAuth _authRepo = authRepo;

        // Login User
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDto body)
        {
            var user = await _authRepo.LoginAsync(body);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            var token = await _authRepo.GenerateToken(user);
            if (token == null)
            {
                 return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Token generation failed." });
            }

            return Ok(new ResponseDto
            {
                Id = user.Id.ToString(),
                FullName = user.FullName,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Token = token
            });
        }
    
    }
}
