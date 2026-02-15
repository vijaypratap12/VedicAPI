using System.ComponentModel.DataAnnotations;

namespace VedicAPI.API.Models.DTOs
{
    /// <summary>
    /// DTO for treatment outcome submission
    /// </summary>
    public class TreatmentOutcomeDto
    {
        [Required(ErrorMessage = "Treatment Plan ID is required")]
        public long TreatmentPlanId { get; set; }

        [Required(ErrorMessage = "Patient ID is required")]
        public long PatientId { get; set; }

        [Range(1, 10, ErrorMessage = "Effectiveness score must be between 1 and 10")]
        public int? EffectivenessScore { get; set; }

        public string? SideEffects { get; set; }
        public string? PatientFeedback { get; set; }
        public string? DoctorNotes { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public int? RecordedBy { get; set; }
    }
}
