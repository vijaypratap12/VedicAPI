namespace VedicAPI.API.Models.DTOs
{
    /// <summary>
    /// DTO for full chapter details including HTML content
    /// </summary>
    public class ChapterDetailDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string BookAuthor { get; set; } = string.Empty;
        public int ChapterNumber { get; set; }
        public string ChapterTitle { get; set; } = string.Empty;
        public string? ChapterSubtitle { get; set; }
        public string ContentHtml { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public int? WordCount { get; set; }
        public int? ReadingTimeMinutes { get; set; }
        public bool HasPreviousChapter { get; set; }
        public bool HasNextChapter { get; set; }
        public int? PreviousChapterId { get; set; }
        public int? NextChapterId { get; set; }
    }
}

