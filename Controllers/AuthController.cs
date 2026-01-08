using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApi.Entities;
using TaskManagerApi.Repositories;

namespace TaskManagerApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserRepository _users;
        private readonly IConfiguration _config;

        public AuthController(UserRepository users, IConfiguration config)
        {
            _users = users;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromQuery] string email, [FromQuery] string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return BadRequest("Email and password are required.");

            var user = await _users.GetByEmail(email);
            if (user == null || user.Password != password)
                return Unauthorized("Invalid credentials.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: new[] { new Claim("id", user.Id.ToString()) },
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }
}
