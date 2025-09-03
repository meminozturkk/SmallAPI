using System.ComponentModel.DataAnnotations;

namespace SmallAPI.Models.DTOs
{
    public class ResetDto
    {
        /// <summary>
        /// Endpoint'e erişim için gerekli olan token
        /// </summary>
        [Required]
        public required Guid RequestToken { get; set; }

        /// <summary>
        /// Talep edilen e-posta adresi
        /// </summary>
        [Required]
        public required string Email { get; set; }

        /// <summary>
        /// Yeni şifre
        /// </summary>
        [Required]
        public required string NewPassword { get; set; }
    }
}