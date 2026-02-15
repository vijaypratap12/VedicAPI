using System.ComponentModel.DataAnnotations;

namespace VedicAPI.API.Models.DTOs
{
    /// <summary>
    /// DTO for creating a treatment plan
    /// </summary>
    public class TreatmentPlanCreateDto
    {
        [Required(ErrorMessage = "Patient ID is required")]
        public long PatientId { get; set; }

        [Required(ErrorMessage = "Condition ID is required")]
        public long ConditionId { get; set; }

        [Required(ErrorMessage = "Prakriti is required")]
        [StringLength(50, ErrorMessage = "Prakriti cannot exceed 50 characters")]
        public string Prakriti { get; set; } = string.Empty;

        public string? HerbalMedicines { get; set; }
        public string? YogaAsanas { get; set; }
        public string? DietaryRecommendations { get; set; }
        public string? LifestyleModifications { get; set; }

        [StringLength(100, ErrorMessage = "Duration cannot exceed 100 characters")]
        public string? Duration { get; set; }

        [Range(0, 100, ErrorMessage = "Confidence score must be between 0 and 100")]
        public decimal? ConfidenceScore { get; set; }

        public string? Explanation { get; set; }
        public int? CreatedBy { get; set; }
    }
}
