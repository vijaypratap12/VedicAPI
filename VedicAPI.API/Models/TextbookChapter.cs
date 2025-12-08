namespace VedicAPI.API.Models
{
    /// <summary>
    /// Represents a textbook chapter entity in the database
    /// </summary>
    public class TextbookChapter
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
        public bool IsActive { get; set; } = true;

        // Navigation property
        public Textbook? Textbook { get; set; }
    }
}

