using SmallAPI.Data;
using SmallAPI.Models;
using SmallAPI.Repositories.Interfaces;

namespace SmallAPI.Repositories
{
    public class PasswordResetRepository : Repository<PasswordReset>, IPasswordResetRepository
    {
        public PasswordResetRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}