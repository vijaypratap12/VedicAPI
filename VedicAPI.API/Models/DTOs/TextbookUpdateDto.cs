using System.ComponentModel.DataAnnotations;

namespace VedicAPI.API.Models.DTOs
{
    /// <summary>
    /// DTO for updating an existing textbook
    /// </summary>
    public class TextbookUpdateDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(500, ErrorMessage = "Title cannot exceed 500 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Author is required")]
        [StringLength(200, ErrorMessage = "Author name cannot exceed 200 characters")]
        public string Author { get; set; } = string.Empty;

        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
        public string? Description { get; set; }

        [StringLength(500, ErrorMessage = "Cover image URL cannot exceed 500 characters")]
        public string? CoverImageUrl { get; set; }

        public int TotalChapters { get; set; }

        [StringLength(100, ErrorMessage = "Category cannot exceed 100 characters")]
        public string? Category { get; set; }

        [StringLength(50, ErrorMessage = "Language cannot exceed 50 characters")]
        public string? Language { get; set; }

        [Range(1, 9999, ErrorMessage = "Publication year must be between 1 and 9999")]
        public int? PublicationYear { get; set; }

        [StringLength(20, ErrorMessage = "ISBN cannot exceed 20 characters")]
        public string? ISBN { get; set; }

        // Textbook-specific fields
        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5")]
        public decimal? Rating { get; set; }

        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters")]
        public string? Status { get; set; }

        [StringLength(1000, ErrorMessage = "Tags cannot exceed 1000 characters")]
        public string? Tags { get; set; }

        [StringLength(50, ErrorMessage = "Level cannot exceed 50 characters")]
        public string? Level { get; set; }

        [StringLength(50, ErrorMessage = "Year cannot exceed 50 characters")]
        public string? Year { get; set; }

        [Range(1, 10000, ErrorMessage = "Page count must be between 1 and 10000")]
        public int? PageCount { get; set; }

        public bool IsActive { get; set; } = true;
    }
}

