using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagerApi.Entities;

namespace TaskManagerApi.Repositories;

public interface IUserRepository
{
    Task<User?> Get(int id);
    Task<User?> GetByEmail(string email);
    Task Add(User user);
    Task<IEnumerable<User>> GetAll(int page, int pageSize);
}
