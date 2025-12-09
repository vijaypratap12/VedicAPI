using System.ComponentModel.DataAnnotations;

namespace VedicAPI.API.Models.DTOs
{
    /// <summary>
    /// DTO for updating a Thesis
    /// </summary>
    public class ThesisUpdateDto
    {
        [StringLength(500, ErrorMessage = "Title cannot exceed 500 characters")]
        public string? Title { get; set; }

        [StringLength(300, ErrorMessage = "Author cannot exceed 300 characters")]
        public string? Author { get; set; }

        [StringLength(500, ErrorMessage = "Guide names cannot exceed 500 characters")]
        public string? GuideNames { get; set; }

        [StringLength(300, ErrorMessage = "Institution cannot exceed 300 characters")]
        public string? Institution { get; set; }

        [StringLength(200, ErrorMessage = "Department cannot exceed 200 characters")]
        public string? Department { get; set; }

        [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100")]
        public int? Year { get; set; }

        [StringLength(100, ErrorMessage = "Category cannot exceed 100 characters")]
        public string? Category { get; set; }

        [StringLength(100, ErrorMessage = "Thesis type cannot exceed 100 characters")]
        public string? ThesisType { get; set; }

        public string? Abstract { get; set; }
        public string? ContentHtml { get; set; }

        [StringLength(500, ErrorMessage = "Keywords cannot exceed 500 characters")]
        public string? Keywords { get; set; }

        [Range(0, 10000, ErrorMessage = "Pages must be between 0 and 10000")]
        public int? Pages { get; set; }

        public DateTime? SubmissionDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? DefenseDate { get; set; }

        [StringLength(50, ErrorMessage = "Grade cannot exceed 50 characters")]
        public string? Grade { get; set; }

        [Url(ErrorMessage = "PDF URL must be a valid URL")]
        [StringLength(500, ErrorMessage = "PDF URL cannot exceed 500 characters")]
        public string? PdfUrl { get; set; }

        [Url(ErrorMessage = "Cover image URL must be a valid URL")]
        [StringLength(500, ErrorMessage = "Cover image URL cannot exceed 500 characters")]
        public string? CoverImageUrl { get; set; }

        [StringLength(100, ErrorMessage = "University registration number cannot exceed 100 characters")]
        public string? UniversityRegistrationNumber { get; set; }

        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5")]
        public decimal? Rating { get; set; }

        public string? Status { get; set; }
        public bool? IsFeatured { get; set; }
        public bool? IsActive { get; set; }
    }
}

