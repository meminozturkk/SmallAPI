using SmallAPI.Models.DTOs;

namespace SmallAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> ResetPassword(ResetDto resetDto);
    }
} 