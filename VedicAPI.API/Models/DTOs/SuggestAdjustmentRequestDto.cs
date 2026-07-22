using System.Collections.Generic;

namespace VedicAPI.API.Models.DTOs
{
    /// <summary>
    /// DTO for requesting AI plan adjustments based on outcomes
    /// </summary>
    public class SuggestAdjustmentRequestDto
    {
        public long PatientId { get; set; }
        public long ConditionId { get; set; }
        public TreatmentPlanResponseDto CurrentPlan { get; set; } = new();
        public List<TreatmentOutcomeDto> Outcomes { get; set; } = new();
    }
}
