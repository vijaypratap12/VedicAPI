namespace VedicAPI.API.Models.DTOs
{
    /// <summary>
    /// DTO for Research Paper response (without full content)
    /// </summary>
    public class ResearchPaperResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<string> Authors { get; set; } = new(); // Parsed from comma-separated
        public string Institution { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Abstract { get; set; } = string.Empty;
        public List<string> Keywords { get; set; } = new(); // Parsed from comma-separated
        public int Pages { get; set; }
        public int DownloadCount { get; set; }
        public int ViewCount { get; set; }
        public decimal Rating { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? DOI { get; set; }
        public string? JournalName { get; set; }
        public string? Volume { get; set; }
        public string? IssueNumber { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string? PdfUrl { get; set; }
        public string? CoverImageUrl { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

