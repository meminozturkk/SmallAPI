using System.ComponentModel.DataAnnotations;

namespace SmallAPI.Models
{
    public class User : BaseEntity
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        public bool IsAdmin { get; set; } = false;

        public string? FcmToken { get; set; }

        // Computed properties
        public string FullName => $"{FirstName} {LastName}";

     
    }
} 