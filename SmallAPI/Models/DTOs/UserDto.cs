namespace SmallAPI.Models.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public string? FcmToken { get; set; } = null;
        public int IconIndex { get; set; } = 0; // Kullanıcı ikonu indeksi
        public DateTime CreatedAt { get; set; }
        
        // Öğrenci özellikleri
        public string FullName { get; set; } = string.Empty;
        
        // İsteğe bağlı: ilerleme bilgileri
        public int? TotalProgress { get; set; }
        public int? CorrectAnswers { get; set; }
        public int? TotalAnswers { get; set; }
        public bool? LastLoginCompletedTask { get; set; }
    }
} 