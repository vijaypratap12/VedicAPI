namespace VedicAPI.API.Models
{
    /// <summary>
    /// Represents a medical condition entity
    /// </summary>
    public class Condition
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? SanskritName { get; set; }
        public string Category { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? CommonSymptoms { get; set; }
        public string? AffectedDoshas { get; set; }
        public string? Severity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
