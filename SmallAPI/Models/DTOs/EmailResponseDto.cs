namespace SmallAPI.Models.DTOs
{
    public class EmailResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? RequestToken { get; set; } = null;
    }
} 