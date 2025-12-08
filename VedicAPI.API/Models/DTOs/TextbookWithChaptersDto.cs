namespace VedicAPI.API.Models.DTOs
{
    /// <summary>
    /// DTO for textbook with list of chapter summaries
    /// </summary>
    public class TextbookWithChaptersDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? CoverImageUrl { get; set; }
        public int TotalChapters { get; set; }
        public string? Category { get; set; }
        public string? Language { get; set; }
        public int? PublicationYear { get; set; }
        public string? ISBN { get; set; }
        
        // Textbook-specific fields
        public decimal? Rating { get; set; }
        public int DownloadCount { get; set; }
        public int ViewCount { get; set; }
        public string? Status { get; set; }
        public string? Tags { get; set; }
        public string? Level { get; set; }
        public string? Year { get; set; }
        public int? PageCount { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public List<TextbookChapterSummaryDto> Chapters { get; set; } = new();
    }
}

