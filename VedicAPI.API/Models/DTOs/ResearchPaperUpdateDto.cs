using System.ComponentModel.DataAnnotations;

namespace VedicAPI.API.Models.DTOs
{
    /// <summary>
    /// DTO for updating a Research Paper
    /// </summary>
    public class ResearchPaperUpdateDto
    {
        [StringLength(500, ErrorMessage = "Title cannot exceed 500 characters")]
        public string? Title { get; set; }

        [StringLength(1000, ErrorMessage = "Authors cannot exceed 1000 characters")]
        public string? Authors { get; set; }

        [StringLength(300, ErrorMessage = "Institution cannot exceed 300 characters")]
        public string? Institution { get; set; }

        [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100")]
        public int? Year { get; set; }

        [StringLength(100, ErrorMessage = "Category cannot exceed 100 characters")]
        public string? Category { get; set; }

        public string? Abstract { get; set; }
        public string? ContentHtml { get; set; }

        [StringLength(500, ErrorMessage = "Keywords cannot exceed 500 characters")]
        public string? Keywords { get; set; }

        [Range(0, 10000, ErrorMessage = "Pages must be between 0 and 10000")]
        public int? Pages { get; set; }

        [StringLength(200, ErrorMessage = "DOI cannot exceed 200 characters")]
        public string? DOI { get; set; }

        [StringLength(300, ErrorMessage = "Journal name cannot exceed 300 characters")]
        public string? JournalName { get; set; }

        [StringLength(50, ErrorMessage = "Volume cannot exceed 50 characters")]
        public string? Volume { get; set; }

        [StringLength(50, ErrorMessage = "Issue number cannot exceed 50 characters")]
        public string? IssueNumber { get; set; }

        public DateTime? PublicationDate { get; set; }

        [Url(ErrorMessage = "PDF URL must be a valid URL")]
        [StringLength(500, ErrorMessage = "PDF URL cannot exceed 500 characters")]
        public string? PdfUrl { get; set; }

        [Url(ErrorMessage = "Cover image URL must be a valid URL")]
        [StringLength(500, ErrorMessage = "Cover image URL cannot exceed 500 characters")]
        public string? CoverImageUrl { get; set; }

        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5")]
        public decimal? Rating { get; set; }

        public string? Status { get; set; }
        public bool? IsFeatured { get; set; }
        public bool? IsActive { get; set; }
    }
}

