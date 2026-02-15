namespace VedicAPI.API.Models
{
    /// <summary>
    /// Represents a yoga asana entity
    /// </summary>
    public class YogaAsana
    {
        public long Id { get; set; }
        public string AsanaName { get; set; } = string.Empty;
        public string? SanskritName { get; set; }
        public string Category { get; set; } = string.Empty;
        public string? Benefits { get; set; }
        public string? Duration { get; set; }
        public string? Difficulty { get; set; }
        public string? Instructions { get; set; }
        public string? Precautions { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public string? VataEffect { get; set; }
        public string? PittaEffect { get; set; }
        public string? KaphaEffect { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
