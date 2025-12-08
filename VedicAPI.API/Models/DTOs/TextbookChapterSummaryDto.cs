namespace VedicAPI.API.Models.DTOs
{
    /// <summary>
    /// DTO for textbook chapter summary (without full content)
    /// </summary>
    public class TextbookChapterSummaryDto
    {
        public int Id { get; set; }
        public int TextbookId { get; set; }
        public int ChapterNumber { get; set; }
        public string ChapterTitle { get; set; } = string.Empty;
        public string? ChapterSubtitle { get; set; }
        public string? Summary { get; set; }
        public int? WordCount { get; set; }
        public int? ReadingTimeMinutes { get; set; }
        public int DisplayOrder { get; set; }
    }
}

