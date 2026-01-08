using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;
using TaskManagerApi.Entities;

namespace TaskManagerApi.Repositories
{
    public class UserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {

            _context = context;
        }

        public async Task<User> Add(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetById(int id)
        {

            return await _context.Users.FindAsync(id);

        }

        public async Task<User> GetByEmail(string email)
        {

            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<(List<User>, int total)> GetAll(int page, int pageSize)
        {
            var total = await _context.Users.CountAsync();
            var users = await _context.Users
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (users, total);
        }
    }
}
