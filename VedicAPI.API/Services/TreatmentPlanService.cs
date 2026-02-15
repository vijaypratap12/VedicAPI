using System.Text.Json;
using VedicAPI.API.Models;
using VedicAPI.API.Models.DTOs;
using VedicAPI.API.Repositories.Interfaces;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Services
{
    /// <summary>
    /// Service implementation for Treatment plan business logic
    /// </summary>
    public class TreatmentPlanService : ITreatmentPlanService
    {
        private readonly ITreatmentRepository _treatmentRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IConditionRepository _conditionRepository;
        private readonly ILogger<TreatmentPlanService> _logger;

        public TreatmentPlanService(
            ITreatmentRepository treatmentRepository,
            IPatientRepository patientRepository,
            IConditionRepository conditionRepository,
            ILogger<TreatmentPlanService> logger)
        {
            _treatmentRepository = treatmentRepository;
            _patientRepository = patientRepository;
            _conditionRepository = conditionRepository;
            _logger = logger;
        }

        public async Task<TreatmentPlanResponseDto?> GetTreatmentPlanByIdAsync(long id)
        {
            try
            {
                var plan = await _treatmentRepository.GetTreatmentPlanByIdAsync(id);
                if (plan == null) return null;

                // Get related entities for complete response
                var patient = await _patientRepository.GetByIdAsync(plan.PatientId);
                var condition = await _conditionRepository.GetByIdAsync(plan.ConditionId);

                return MapToResponseDto(plan, patient?.Name ?? "Unknown", condition?.Name ?? "Unknown");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTreatmentPlanByIdAsync for ID {PlanId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<TreatmentPlanResponseDto>> GetTreatmentPlansByPatientIdAsync(long patientId)
        {
            try
            {
                var plans = await _treatmentRepository.GetTreatmentPlansByPatientIdAsync(patientId);
                var patient = await _patientRepository.GetByIdAsync(patientId);
                var patientName = patient?.Name ?? "Unknown";

                var responseDtos = new List<TreatmentPlanResponseDto>();

                foreach (var plan in plans)
                {
                    var condition = await _conditionRepository.GetByIdAsync(plan.ConditionId);
                    responseDtos.Add(MapToResponseDto(plan, patientName, condition?.Name ?? "Unknown"));
                }

                return responseDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTreatmentPlansByPatientIdAsync for patient {PatientId}", patientId);
                throw;
            }
        }

        public async Task<TreatmentPlanResponseDto> CreateTreatmentPlanAsync(TreatmentPlanCreateDto planDto)
        {
            try
            {
                var plan = MapToEntity(planDto);
                var planId = await _treatmentRepository.CreateTreatmentPlanAsync(plan);
                plan.Id = planId;

                // Get related entities
                var patient = await _patientRepository.GetByIdAsync(plan.PatientId);
                var condition = await _conditionRepository.GetByIdAsync(plan.ConditionId);

                _logger.LogInformation("Treatment plan created successfully with ID {PlanId}", planId);
                return MapToResponseDto(plan, patient?.Name ?? "Unknown", condition?.Name ?? "Unknown");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateTreatmentPlanAsync for patient {PatientId}", planDto.PatientId);
                throw;
            }
        }

        public async Task<TreatmentPlanResponseDto?> UpdateTreatmentPlanAsync(long id, TreatmentPlanCreateDto planDto)
        {
            try
            {
                var existingPlan = await _treatmentRepository.GetTreatmentPlanByIdAsync(id);
                if (existingPlan == null)
                {
                    _logger.LogWarning("Treatment plan with ID {PlanId} not found for update", id);
                    return null;
                }

                var updatedPlan = MapToEntity(planDto, id);
                updatedPlan.CreatedAt = existingPlan.CreatedAt;
                updatedPlan.PatientId = existingPlan.PatientId;
                updatedPlan.ConditionId = existingPlan.ConditionId;

                var success = await _treatmentRepository.UpdateTreatmentPlanAsync(updatedPlan);
                if (success)
                {
                    var patient = await _patientRepository.GetByIdAsync(updatedPlan.PatientId);
                    var condition = await _conditionRepository.GetByIdAsync(updatedPlan.ConditionId);

                    _logger.LogInformation("Treatment plan with ID {PlanId} updated successfully", id);
                    return MapToResponseDto(updatedPlan, patient?.Name ?? "Unknown", condition?.Name ?? "Unknown");
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateTreatmentPlanAsync for plan ID {PlanId}", id);
                throw;
            }
        }

        public async Task<long> SaveTreatmentOutcomeAsync(TreatmentOutcomeDto outcomeDto)
        {
            try
            {
                var outcome = MapOutcomeToEntity(outcomeDto);
                var outcomeId = await _treatmentRepository.SaveTreatmentOutcomeAsync(outcome);

                _logger.LogInformation("Treatment outcome saved successfully with ID {OutcomeId}", outcomeId);
                return outcomeId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SaveTreatmentOutcomeAsync for plan {PlanId}", outcomeDto.TreatmentPlanId);
                throw;
            }
        }

        #region Mapping Methods

        private static TreatmentPlanResponseDto MapToResponseDto(TreatmentPlan plan, string patientName, string conditionName)
        {
            return new TreatmentPlanResponseDto
            {
                Id = plan.Id,
                PatientId = plan.PatientId,
                PatientName = patientName,
                ConditionId = plan.ConditionId,
                ConditionName = conditionName,
                Prakriti = plan.Prakriti,
                HerbalMedicines = plan.HerbalMedicines,
                YogaAsanas = plan.YogaAsanas,
                DietaryRecommendations = plan.DietaryRecommendations,
                LifestyleModifications = plan.LifestyleModifications,
                Duration = plan.Duration,
                ConfidenceScore = plan.ConfidenceScore,
                Explanation = plan.Explanation,
                CreatedAt = plan.CreatedAt,
                UpdatedAt = plan.UpdatedAt
            };
        }

        private static TreatmentPlan MapToEntity(TreatmentPlanCreateDto dto)
        {
            return new TreatmentPlan
            {
                PatientId = dto.PatientId,
                ConditionId = dto.ConditionId,
                Prakriti = dto.Prakriti,
                HerbalMedicines = dto.HerbalMedicines,
                YogaAsanas = dto.YogaAsanas,
                DietaryRecommendations = dto.DietaryRecommendations,
                LifestyleModifications = dto.LifestyleModifications,
                Duration = dto.Duration,
                ConfidenceScore = dto.ConfidenceScore,
                Explanation = dto.Explanation,
                CreatedBy = dto.CreatedBy,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
        }

        private static TreatmentPlan MapToEntity(TreatmentPlanCreateDto dto, long id)
        {
            return new TreatmentPlan
            {
                Id = id,
                HerbalMedicines = dto.HerbalMedicines,
                YogaAsanas = dto.YogaAsanas,
                DietaryRecommendations = dto.DietaryRecommendations,
                LifestyleModifications = dto.LifestyleModifications,
                Duration = dto.Duration,
                ConfidenceScore = dto.ConfidenceScore,
                Explanation = dto.Explanation,
                UpdatedAt = DateTime.UtcNow
            };
        }

        private static TreatmentOutcome MapOutcomeToEntity(TreatmentOutcomeDto dto)
        {
            return new TreatmentOutcome
            {
                TreatmentPlanId = dto.TreatmentPlanId,
                PatientId = dto.PatientId,
                EffectivenessScore = dto.EffectivenessScore,
                SideEffects = dto.SideEffects,
                PatientFeedback = dto.PatientFeedback,
                DoctorNotes = dto.DoctorNotes,
                FollowUpDate = dto.FollowUpDate,
                RecordedBy = dto.RecordedBy,
                RecordedAt = DateTime.UtcNow
            };
        }

        #endregion
    }
}
