namespace SmallAPI.Models
{
    public class PasswordReset : BaseEntity
    {
        public Guid? RequestToken { get; set; }
        public required string Email { get; set; }
        public required int Code { get; set; }
        public required DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; }
    }
} 