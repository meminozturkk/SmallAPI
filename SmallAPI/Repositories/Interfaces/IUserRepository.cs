using SmallAPI.Models;

namespace SmallAPI.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        IQueryable<User> Query();
        
       
    }
} 