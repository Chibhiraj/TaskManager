using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using TaskManagerApi.Data;
using TaskManagerApi.Entities;
using TaskStatus = TaskManagerApi.Entities.TaskStatus;

namespace TaskManagerApi.Repositories
{
    public class TaskRepository
    {
        private readonly AppDbContext _context;
        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TaskItem> Add(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TaskItem> GetById(int id)
        {
            return await _context.Tasks.Include(t => t.AssignedTo).FirstOrDefaultAsync(t => t.Id == id);
        }
        public async Task<(List<TaskItem>, int total)> GetAll(int page, int pageSize, TaskStatus? status = null,
            TaskPriority? priority = null, int? assignedToId = null, DateTime? dueBefore = null, DateTime? dueAfter = null
        )
        {
            var query = _context.Tasks.Include(t => t.AssignedTo).AsQueryable();

            if (status.HasValue) query = query.Where(t => t.Status == status);
            if (priority.HasValue) query = query.Where(t => t.Priority == priority);
            if (assignedToId.HasValue) query = query.Where(t => t.AssignedToId == assignedToId);
            if (dueBefore.HasValue) query = query.Where(t => t.DueDate <= dueBefore);
            if (dueAfter.HasValue) query = query.Where(t => t.DueDate >= dueAfter);

            var total = await query.CountAsync();

            var tasks = await query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (tasks, total);
        }

        public async Task Update(TaskItem task)
        {
            task.UpdatedAt = DateTime.UtcNow;
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(TaskItem task)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}
