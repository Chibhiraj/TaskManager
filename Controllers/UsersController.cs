using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.DTOs;
using TaskManagerApi.Entities;
using TaskManagerApi.Repositories;

namespace TaskManagerApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository _repo;

        public UsersController(UserRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); 

            // Check for duplicate email
            if (await _repo.GetByEmail(dto.Email) != null)
                return Conflict(new { message = "Email already exists." });

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = dto.Password
            };

            await _repo.Add(user);

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var (users, total) = await _repo.GetAll(page, pageSize);
            return Ok(new { total, page, pageSize, users });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _repo.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}
