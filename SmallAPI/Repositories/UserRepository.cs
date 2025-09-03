using SmallAPI.Data;
using SmallAPI.Models;
using SmallAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SmallAPI.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email == email);
        }

        public IQueryable<User> Query()
        {
            return _context.Users.AsQueryable();
        }

     
    }
}