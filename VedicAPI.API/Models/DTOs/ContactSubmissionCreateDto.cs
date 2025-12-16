using System.ComponentModel.DataAnnotations;

namespace VedicAPI.API.Models.DTOs
{
    public class ContactSubmissionCreateDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
        public string Email { get; set; } = string.Empty;

        [StringLength(300, ErrorMessage = "Organization name cannot exceed 300 characters")]
        public string? Organization { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [StringLength(500, ErrorMessage = "Subject cannot exceed 500 characters")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Message is required")]
        [StringLength(5000, ErrorMessage = "Message cannot exceed 5000 characters")]
        [MinLength(10, ErrorMessage = "Message must be at least 10 characters")]
        public string Message { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact type is required")]
        [RegularExpression("^(general|collaboration|contribution|technical|research|feedback)$", 
            ErrorMessage = "Invalid contact type")]
        public string ContactType { get; set; } = "general";
    }
}

