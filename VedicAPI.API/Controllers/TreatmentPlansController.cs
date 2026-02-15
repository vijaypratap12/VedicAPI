using Microsoft.AspNetCore.Mvc;
using VedicAPI.API.Models.Common;
using VedicAPI.API.Models.DTOs;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Controllers
{
    /// <summary>
    /// Controller for Treatment plan operations
    /// </summary>
    [ApiController]
    [Route("api/treatment-plans")]
    public class TreatmentPlansController : ControllerBase
    {
        private readonly ITreatmentPlanService _treatmentPlanService;
        private readonly ILogger<TreatmentPlansController> _logger;

        public TreatmentPlansController(
            ITreatmentPlanService treatmentPlanService,
            ILogger<TreatmentPlansController> logger)
        {
            _treatmentPlanService = treatmentPlanService;
            _logger = logger;
        }

        /// <summary>
        /// Get treatment plan by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<TreatmentPlanResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTreatmentPlanById(long id)
        {
            try
            {
                var plan = await _treatmentPlanService.GetTreatmentPlanByIdAsync(id);
                if (plan == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse($"Treatment plan with ID {id} not found"));
                }

                return Ok(ApiResponse<TreatmentPlanResponseDto>.SuccessResponse(
                    plan,
                    "Treatment plan retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving treatment plan with ID {PlanId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving the treatment plan",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get all treatment plans for a patient
        /// </summary>
        [HttpGet("patient/{patientId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TreatmentPlanResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTreatmentPlansByPatientId(long patientId)
        {
            try
            {
                var plans = await _treatmentPlanService.GetTreatmentPlansByPatientIdAsync(patientId);
                return Ok(ApiResponse<IEnumerable<TreatmentPlanResponseDto>>.SuccessResponse(
                    plans,
                    "Treatment plans retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving treatment plans for patient {PatientId}", patientId);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving treatment plans",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Create a new treatment plan
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<TreatmentPlanResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTreatmentPlan([FromBody] TreatmentPlanCreateDto planDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse(
                        "Invalid treatment plan data",
                        ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()));
                }

                var plan = await _treatmentPlanService.CreateTreatmentPlanAsync(planDto);
                return CreatedAtAction(
                    nameof(GetTreatmentPlanById),
                    new { id = plan.Id },
                    ApiResponse<TreatmentPlanResponseDto>.SuccessResponse(
                        plan,
                        "Treatment plan created successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating treatment plan");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while creating the treatment plan",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Update an existing treatment plan
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<TreatmentPlanResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTreatmentPlan(long id, [FromBody] TreatmentPlanCreateDto planDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse(
                        "Invalid treatment plan data",
                        ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()));
                }

                var plan = await _treatmentPlanService.UpdateTreatmentPlanAsync(id, planDto);
                if (plan == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse($"Treatment plan with ID {id} not found"));
                }

                return Ok(ApiResponse<TreatmentPlanResponseDto>.SuccessResponse(
                    plan,
                    "Treatment plan updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating treatment plan with ID {PlanId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while updating the treatment plan",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Submit treatment outcome/feedback
        /// </summary>
        [HttpPost("{id}/outcome")]
        [ProducesResponseType(typeof(ApiResponse<long>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SubmitOutcome(long id, [FromBody] TreatmentOutcomeDto outcomeDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse(
                        "Invalid outcome data",
                        ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()));
                }

                // Ensure the treatment plan ID matches
                outcomeDto.TreatmentPlanId = id;

                var outcomeId = await _treatmentPlanService.SaveTreatmentOutcomeAsync(outcomeDto);
                return CreatedAtAction(
                    nameof(GetTreatmentPlanById),
                    new { id = id },
                    ApiResponse<long>.SuccessResponse(
                        outcomeId,
                        "Treatment outcome saved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting outcome for treatment plan {PlanId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while saving the outcome",
                    new List<string> { ex.Message }));
            }
        }
    }
}
