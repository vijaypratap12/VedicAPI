namespace VedicAPI.API.Models
{
    /// <summary>
    /// Represents a Prakriti assessment question entity
    /// </summary>
    public class PrakritiQuestion
    {
        public long Id { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Question { get; set; } = string.Empty;
        public string VataOption { get; set; } = string.Empty;
        public string PittaOption { get; set; } = string.Empty;
        public string KaphaOption { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
    }
}
