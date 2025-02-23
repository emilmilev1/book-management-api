using BookManagementApi.Models;
using BookManagementApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;

        public AuthController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            if (user.Username == "admin" && user.Password == "password")
            {
                var token = _jwtService.GenerateToken(user.Username);
                
                return Ok(new { Token = token });
            }

            return Unauthorized(new { message = "Invalid credentials" });
        }
    }
}