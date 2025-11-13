namespace VedicAPI.API.Models
{
    /// <summary>
    /// Represents a chapter of a book
    /// </summary>
    public class BookChapter
    {
        public int Id { get; set; }
        public int BookId { get; set; }
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
        public Book? Book { get; set; }
    }
}

