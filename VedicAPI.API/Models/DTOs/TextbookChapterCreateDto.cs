using System.ComponentModel.DataAnnotations;

namespace VedicAPI.API.Models.DTOs
{
    /// <summary>
    /// DTO for creating a new textbook chapter
    /// </summary>
    public class TextbookChapterCreateDto
    {
        [Required(ErrorMessage = "Chapter number is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Chapter number must be positive")]
        public int ChapterNumber { get; set; }

        [Required(ErrorMessage = "Chapter title is required")]
        [StringLength(500, ErrorMessage = "Chapter title cannot exceed 500 characters")]
        public string ChapterTitle { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Chapter subtitle cannot exceed 500 characters")]
        public string? ChapterSubtitle { get; set; }

        [Required(ErrorMessage = "Content HTML is required")]
        public string ContentHtml { get; set; } = string.Empty;

        [StringLength(2000, ErrorMessage = "Summary cannot exceed 2000 characters")]
        public string? Summary { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Word count must be non-negative")]
        public int? WordCount { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Reading time must be non-negative")]
        public int? ReadingTimeMinutes { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Display order must be non-negative")]
        public int? DisplayOrder { get; set; }
    }
}

