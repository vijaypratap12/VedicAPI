namespace VedicAPI.API.Models.DTOs
{
    /// <summary>
    /// DTO for dietary item
    /// </summary>
    public class DietaryItemDto
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
    }
}
