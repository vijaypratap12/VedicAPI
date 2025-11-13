using System.ComponentModel.DataAnnotations;

namespace VedicAPI.API.Models.DTOs
{
    /// <summary>
    /// DTO for updating an existing chapter
    /// </summary>
    public class ChapterUpdateDto
    {
        [Required(ErrorMessage = "Chapter title is required")]
        [StringLength(500, ErrorMessage = "Chapter title cannot exceed 500 characters")]
        public string ChapterTitle { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Chapter subtitle cannot exceed 500 characters")]
        public string? ChapterSubtitle { get; set; }

        [Required(ErrorMessage = "Content HTML is required")]
        public string ContentHtml { get; set; } = string.Empty;

        [StringLength(2000, ErrorMessage = "Summary cannot exceed 2000 characters")]
        public string? Summary { get; set; }

        public int? WordCount { get; set; }

        public int? ReadingTimeMinutes { get; set; }

        public int? DisplayOrder { get; set; }

        public bool IsActive { get; set; } = true;
    }
}

