using SmallAPI.Models;

namespace SmallAPI.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        bool ValidateToken(string token);
    }
} 