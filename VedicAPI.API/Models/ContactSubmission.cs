using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VedicAPI.API.Models
{
    [Table("ContactSubmissions")]
    public class ContactSubmission
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [MaxLength(300)]
        public string? Organization { get; set; }

        [Required]
        [MaxLength(500)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string ContactType { get; set; } = "general"; // general, collaboration, contribution, technical, research, feedback

        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, InProgress, Resolved, Closed

        [MaxLength(20)]
        public string Priority { get; set; } = "Normal"; // Low, Normal, High, Urgent

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        public DateTime? RespondedAt { get; set; }

        [MaxLength(200)]
        public string? RespondedBy { get; set; }

        public string? ResponseMessage { get; set; }

        public string? Notes { get; set; }

        public bool IsArchived { get; set; } = false;

        [MaxLength(50)]
        public string? IpAddress { get; set; }

        [MaxLength(500)]
        public string? UserAgent { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

