namespace VedicAPI.API.Models.DTOs
{
    /// <summary>
    /// DTO for treatment plan response
    /// </summary>
    public class TreatmentPlanResponseDto
    {
        public long Id { get; set; }
        public long PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public long ConditionId { get; set; }
        public string ConditionName { get; set; } = string.Empty;
        public string Prakriti { get; set; } = string.Empty;
        public string? HerbalMedicines { get; set; }
        public string? YogaAsanas { get; set; }
        public string? DietaryRecommendations { get; set; }
        public string? LifestyleModifications { get; set; }
        public string? Duration { get; set; }
        public decimal? ConfidenceScore { get; set; }
        public string? Explanation { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
