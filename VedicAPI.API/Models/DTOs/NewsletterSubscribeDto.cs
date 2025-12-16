using System.ComponentModel.DataAnnotations;

namespace VedicAPI.API.Models.DTOs
{
    public class NewsletterSubscribeDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
        public string Email { get; set; } = string.Empty;

        public string Source { get; set; } = "Contact Page";
    }
}

