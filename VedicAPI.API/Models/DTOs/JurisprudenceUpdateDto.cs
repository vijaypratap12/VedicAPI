using System.ComponentModel.DataAnnotations;

namespace VedicAPI.API.Models.DTOs
{
    /// <summary>
    /// DTO for updating an existing Jurisprudence item
    /// </summary>
    public class JurisprudenceUpdateDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(500, ErrorMessage = "Title cannot exceed 500 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Type is required")]
        [StringLength(50, ErrorMessage = "Type cannot exceed 50 characters")]
        public string Type { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "Date cannot exceed 20 characters")]
        public string? Date { get; set; }

        public string? Description { get; set; }

        [StringLength(500, ErrorMessage = "Document URL cannot exceed 500 characters")]
        public string? DocumentUrl { get; set; }

        [StringLength(100, ErrorMessage = "State cannot exceed 100 characters")]
        public string? State { get; set; }

        public int DisplayOrder { get; set; } = 0;
    }
}
