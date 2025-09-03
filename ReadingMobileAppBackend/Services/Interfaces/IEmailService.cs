namespace SmallAPI.Services.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendPasswordResetCodeAsync(string toEmail);
        Task<Guid?> ConfirmResetCode(string userEmail, int code);
    }
}
