using System.ComponentModel.DataAnnotations;

namespace SmallAPI.Models.DTOs
{
    /// <summary>
    /// Kullanıcı giriş bilgileri
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Email adresi
        /// </summary>
        /// <example>test@example.com</example>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Şifre
        /// </summary>
        /// <example>123456</example>
        [Required]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Fcm token (isteğe bağlı)
        /// </summary>

        public string? FcmToken { get; set; }
    }
} 