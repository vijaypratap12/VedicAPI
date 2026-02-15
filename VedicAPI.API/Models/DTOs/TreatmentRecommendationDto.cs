namespace VedicAPI.API.Models.DTOs
{
    /// <summary>
    /// DTO for complete treatment recommendation
    /// </summary>
    public class TreatmentRecommendationDto
    {
        public long PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string Prakriti { get; set; } = string.Empty;
        public long ConditionId { get; set; }
        public string ConditionName { get; set; } = string.Empty;
        public List<HerbalMedicineDto> RecommendedMedicines { get; set; } = new();
        public List<YogaAsanaDto> RecommendedYogaAsanas { get; set; } = new();
        public List<DietaryItemDto> RecommendedDietaryItems { get; set; } = new();
        public List<string> LifestyleModifications { get; set; } = new();
        public decimal ConfidenceScore { get; set; }
        public string Explanation { get; set; } = string.Empty;
    }
}
