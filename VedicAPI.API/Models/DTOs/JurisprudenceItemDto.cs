namespace VedicAPI.API.Models.DTOs
{
    /// <summary>
    /// DTO for Jurisprudence item response
    /// </summary>
    public class JurisprudenceItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string? Date { get; set; }
        public string? Description { get; set; }
        public string? DocumentUrl { get; set; }
        public string? State { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
