namespace VedicAPI.API.Models.DTOs
{
    /// <summary>
    /// DTO for detailed textbook chapter information including full content
    /// </summary>
    public class TextbookChapterDetailDto
    {
        public int Id { get; set; }
        public int TextbookId { get; set; }
        public int ChapterNumber { get; set; }
        public string ChapterTitle { get; set; } = string.Empty;
        public string? ChapterSubtitle { get; set; }
        public string ContentHtml { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public int? WordCount { get; set; }
        public int? ReadingTimeMinutes { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}

