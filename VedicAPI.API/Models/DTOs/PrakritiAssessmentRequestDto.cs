using System.ComponentModel.DataAnnotations;

namespace VedicAPI.API.Models.DTOs
{
    /// <summary>
    /// DTO for submitting Prakriti assessment
    /// </summary>
    public class PrakritiAssessmentRequestDto
    {
        [Required(ErrorMessage = "Patient ID is required")]
        public long PatientId { get; set; }

        [Required(ErrorMessage = "Responses are required")]
        public string Responses { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vata score is required")]
        [Range(0, 100, ErrorMessage = "Vata score must be between 0 and 100")]
        public int VataScore { get; set; }

        [Required(ErrorMessage = "Pitta score is required")]
        [Range(0, 100, ErrorMessage = "Pitta score must be between 0 and 100")]
        public int PittaScore { get; set; }

        [Required(ErrorMessage = "Kapha score is required")]
        [Range(0, 100, ErrorMessage = "Kapha score must be between 0 and 100")]
        public int KaphaScore { get; set; }

        [Required(ErrorMessage = "Dominant Prakriti is required")]
        [StringLength(50, ErrorMessage = "Dominant Prakriti cannot exceed 50 characters")]
        public string DominantPrakriti { get; set; } = string.Empty;
    }
}
