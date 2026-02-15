namespace VedicAPI.API.Models.DTOs
{
    /// <summary>
    /// DTO for medical condition
    /// </summary>
    public class ConditionDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? SanskritName { get; set; }
        public string Category { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? CommonSymptoms { get; set; }
        public string? AffectedDoshas { get; set; }
        public string? Severity { get; set; }
    }
}
