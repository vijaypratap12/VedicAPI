namespace VedicAPI.API.Models
{
    /// <summary>
    /// Represents a dietary item entity
    /// </summary>
    public class DietaryItem
    {
        public long Id { get; set; }
        public string FoodName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string? VataEffect { get; set; }
        public string? PittaEffect { get; set; }
        public string? KaphaEffect { get; set; }
        public string? Properties { get; set; }
        public string? Benefits { get; set; }
        public string? Rasa { get; set; }
        public string? Virya { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
