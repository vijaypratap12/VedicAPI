using System;

namespace VedicAPI.API.Models.DTOs
{
    public class ContactSubmissionResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Organization { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string ContactType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
        public DateTime? RespondedAt { get; set; }
        public string? RespondedBy { get; set; }
        public string? ResponseMessage { get; set; }
        public bool IsArchived { get; set; }
    }
}

