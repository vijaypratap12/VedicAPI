using System;

namespace VedicAPI.API.Models.DTOs
{
    public class NewsletterSubscriptionResponseDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public DateTime SubscribedAt { get; set; }
        public bool IsActive { get; set; }
        public string Source { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}

