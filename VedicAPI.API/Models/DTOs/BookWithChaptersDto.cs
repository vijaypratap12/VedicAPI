namespace VedicAPI.API.Models.DTOs
{
    /// <summary>
    /// DTO for book with list of chapter summaries
    /// </summary>
    public class BookWithChaptersDto
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
        public DateTime CreatedAt { get; set; }
        public List<ChapterSummaryDto> Chapters { get; set; } = new();
    }
}

