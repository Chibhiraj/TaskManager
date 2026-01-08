using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.DTOs;
using TaskManagerApi.Entities;
using TaskManagerApi.Repositories;
using TaskStatus = System.Threading.Tasks.TaskStatus;

namespace TaskManagerApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly TaskRepository _repo;
        private readonly UserRepository _users;

        public TasksController(TaskRepository repo, UserRepository users)
        {
            _repo = repo;
            _users = users;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            User assignedUser = null;
            if (dto.AssignedToId.HasValue)
            {
                assignedUser = await _users.GetById(dto.AssignedToId.Value);
                if (assignedUser == null) return BadRequest("Assigned user not found.");
            }

            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Status = dto.Status,
                Priority = dto.Priority,
                DueDate = dto.DueDate,
                AssignedTo = assignedUser,
                AssignedToId = dto.AssignedToId
            };

            await _repo.Add(task);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] TaskManagerApi.Entities.TaskStatus? status = null,
            [FromQuery] TaskPriority? priority = null,
            [FromQuery] int? assignedToId = null,
            [FromQuery] DateTime? dueBefore = null,
            [FromQuery] DateTime? dueAfter = null)
        {
            var result = await _repo.GetAll(page, pageSize, status, priority, assignedToId, dueBefore, dueAfter);
            var tasks = result.Item1;
            var total = result.Item2;

            return Ok(new { total, page, pageSize, tasks });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _repo.GetById(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDto dto)
        {
            var task = await _repo.GetById(id);
            if (task == null) return NotFound();

            User assignedUser = null;
            if (dto.AssignedToId.HasValue)
            {
                assignedUser = await _users.GetById(dto.AssignedToId.Value);
                if (assignedUser == null) return BadRequest("Assigned user not found.");
            }

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.Status = dto.Status;
            task.Priority = dto.Priority;
            task.DueDate = dto.DueDate;
            task.AssignedTo = assignedUser;
            task.AssignedToId = dto.AssignedToId;

            await _repo.Update(task);
            return Ok(task);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateTaskStatusDto dto)
        {
            var task = await _repo.GetById(id);
            if (task == null) return NotFound();

            task.Status = dto.Status;
            await _repo.Update(task);
            return Ok(task);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _repo.GetById(id);
            if (task == null) return NotFound();

            await _repo.Delete(task);
            return NoContent();
        }
    }
}
