namespace VedicAPI.API.Models
{
    /// <summary>
    /// Represents a treatment plan entity
    /// </summary>
    public class TreatmentPlan
    {
        public long Id { get; set; }
        public long PatientId { get; set; }
        public long ConditionId { get; set; }
        public string Prakriti { get; set; } = string.Empty;
        public string? HerbalMedicines { get; set; }
        public string? YogaAsanas { get; set; }
        public string? DietaryRecommendations { get; set; }
        public string? LifestyleModifications { get; set; }
        public string? Duration { get; set; }
        public decimal? ConfidenceScore { get; set; }
        public string? Explanation { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
