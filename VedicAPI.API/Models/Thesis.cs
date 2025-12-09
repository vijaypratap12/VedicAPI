namespace VedicAPI.API.Models
{
    /// <summary>
    /// Thesis domain model
    /// </summary>
    public class Thesis
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string? GuideNames { get; set; } // Comma-separated
        public string Institution { get; set; } = string.Empty;
        public string? Department { get; set; }
        public int Year { get; set; }
        public string Category { get; set; } = string.Empty;
        public string? ThesisType { get; set; }
        public string Abstract { get; set; } = string.Empty;
        public string? ContentHtml { get; set; }
        public string? Keywords { get; set; } // Comma-separated
        public int Pages { get; set; }
        public int DownloadCount { get; set; }
        public int ViewCount { get; set; }
        public decimal Rating { get; set; }
        public string Status { get; set; } = "published";
        public DateTime? SubmissionDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? DefenseDate { get; set; }
        public string? Grade { get; set; }
        public string? PdfUrl { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? UniversityRegistrationNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }
    }
}

