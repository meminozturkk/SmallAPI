namespace SmallAPI.Models.DTOs
{
    public class ConfirmResetPasswordDto
    {
        public required string Email { get; set; }
        public required int ResetCode { get; set; }
    }
}