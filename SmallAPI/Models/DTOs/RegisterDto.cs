using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SmallAPI.Models.DTOs
{
    /// <summary>
    /// Kullanıcı kayıt bilgileri
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// Email adresi
        /// </summary>
        /// <example>test@example.com</example>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Şifre (en az 6 karakter)
        /// </summary>
        /// <example>123456</example>
        [Required]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Ad
        /// </summary>
        /// <example>Ahmet</example>
        [Required]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Soyad
        /// </summary>
        /// <example>Yılmaz</example>
        [Required]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Firebase Bildirim Tokeni (isteğe bağlı)
        /// </summary>  
        public string? FcmToken { get; set; } = null;
    }
}