namespace VedicAPI.API.Models
{
    /// <summary>
    /// Represents a patient entity in the database
    /// </summary>
    public class Patient
    {
        public long Id { get; set; }
        public int? UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Prakriti { get; set; }
        public string? PrakritiScore { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
