using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VedicAPI.API.Models
{
    [Table("NewsletterSubscriptions")]
    public class NewsletterSubscription
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public DateTime SubscribedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        public Guid UnsubscribeToken { get; set; } = Guid.NewGuid();

        public DateTime? UnsubscribedAt { get; set; }

        [MaxLength(100)]
        public string Source { get; set; } = "Contact Page";

        [MaxLength(50)]
        public string? IpAddress { get; set; }

        public bool ConfirmationSent { get; set; } = false;

        public DateTime? ConfirmationSentAt { get; set; }

        public int EmailsSentCount { get; set; } = 0;

        public DateTime? LastEmailSentAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

