using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagerApi.Entities;

namespace TaskManagerApi.Repositories;

public interface ITaskRepository
{
    Task<TaskItem?> Get(int id);
    Task Add(TaskItem task);
    Task Update(TaskItem task);
    Task Delete(TaskItem task);
    Task<IEnumerable<TaskItem>> GetAll(int page, int pageSize);
}
