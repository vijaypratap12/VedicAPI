namespace VedicAPI.API.Models
{
    /// <summary>
    /// Jurisprudence (Legal & Policy Document) domain model
    /// </summary>
    public class JurisprudenceItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // guideline, regulation, ruling, ethical
        public string? Date { get; set; } // e.g. "2024" or "2023-06"
        public string? Description { get; set; }
        public string? DocumentUrl { get; set; }
        public string? State { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
