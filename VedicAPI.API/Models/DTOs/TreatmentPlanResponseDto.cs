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
        public string? DoctorName { get; set; }
        public DateTime? LastCheckupDate { get; set; }
        public DateTime? UpcomingCheckupDate { get; set; }
        public string? RevisionHistory { get; set; }
        public System.Collections.Generic.List<TreatmentOutcomeDto> Outcomes { get; set; } = new();
    }
}
