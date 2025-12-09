namespace VedicAPI.API.Models.DTOs
{
    /// <summary>
    /// DTO for Thesis response (without full content)
    /// </summary>
    public class ThesisResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public List<string> GuideNames { get; set; } = new(); // Parsed from comma-separated
        public string Institution { get; set; } = string.Empty;
        public string? Department { get; set; }
        public int Year { get; set; }
        public string Category { get; set; } = string.Empty;
        public string? ThesisType { get; set; }
        public string Abstract { get; set; } = string.Empty;
        public List<string> Keywords { get; set; } = new(); // Parsed from comma-separated
        public int Pages { get; set; }
        public int DownloadCount { get; set; }
        public int ViewCount { get; set; }
        public decimal Rating { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? SubmissionDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? DefenseDate { get; set; }
        public string? Grade { get; set; }
        public string? PdfUrl { get; set; }
        public string? CoverImageUrl { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

