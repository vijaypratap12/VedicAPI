namespace VedicAPI.API.Models.DTOs
{
    /// <summary>
    /// DTO for chapter summary (without HTML content)
    /// </summary>
    public class ChapterSummaryDto
    {
        public int Id { get; set; }
        public int ChapterNumber { get; set; }
        public string ChapterTitle { get; set; } = string.Empty;
        public string? ChapterSubtitle { get; set; }
        public string? Summary { get; set; }
        public int? ReadingTimeMinutes { get; set; }
        public bool IsActive { get; set; }
    }
}

