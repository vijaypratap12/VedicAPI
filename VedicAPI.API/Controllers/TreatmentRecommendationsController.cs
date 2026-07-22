using Microsoft.AspNetCore.Mvc;
using VedicAPI.API.Models.Common;
using VedicAPI.API.Models.DTOs;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Controllers
{
    /// <summary>
    /// Controller for Treatment recommendation operations with feature-flagged AI Engine support
    /// </summary>
    [ApiController]
    [Route("api/treatment-recommendations")]
    public class TreatmentRecommendationsController : ControllerBase
    {
        private readonly ITreatmentRecommendationService _recommendationService;
        private readonly ILogger<TreatmentRecommendationsController> _logger;

        public TreatmentRecommendationsController(
            ITreatmentRecommendationService recommendationService,
            ILogger<TreatmentRecommendationsController> logger)
        {
            _recommendationService = recommendationService;
            _logger = logger;
        }

        /// <summary>
        /// Get current feature-flagged AI system configuration & status
        /// </summary>
        [HttpGet("ai-status")]
        [ProducesResponseType(typeof(ApiResponse<AiConfigDto>), StatusCodes.Status200OK)]
        public IActionResult GetAiStatus()
        {
            var config = _recommendationService.GetAiConfig();
            return Ok(ApiResponse<AiConfigDto>.SuccessResponse(config, "AI configuration retrieved successfully"));
        }

        /// <summary>
        /// Generate treatment recommendations for a patient and condition (supports AI and fallback)
        /// </summary>
        [HttpPost("generate")]
        [ProducesResponseType(typeof(ApiResponse<TreatmentRecommendationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GenerateRecommendations(
            [FromQuery] long patientId, 
            [FromQuery] long conditionId,
            [FromQuery] bool useAi = true,
            [FromBody] AiRecommendationRequestDto? body = null)
        {
            try
            {
                var targetPatientId = patientId > 0 ? patientId : body?.PatientId ?? 0;
                var targetConditionId = conditionId > 0 ? conditionId : body?.ConditionId ?? 0;
                var targetUseAi = useAi && (body == null || body.UseAi);
                var customNotes = body?.CustomClinicalNotes;
                var customConditionName = body?.CustomConditionName;

                if (targetPatientId <= 0 || (targetConditionId <= 0 && string.IsNullOrWhiteSpace(customConditionName)))
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse("Invalid patient ID or condition ID/name"));
                }

                var recommendations = await _recommendationService.GenerateRecommendationsAsync(
                    targetPatientId, targetConditionId, targetUseAi, customNotes, customConditionName);

                return Ok(ApiResponse<TreatmentRecommendationDto>.SuccessResponse(
                    recommendations,
                    "Treatment recommendations generated successfully"));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid argument for generating recommendations");
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating recommendations");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while generating recommendations",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Suggest adjustments to an existing treatment plan based on recorded outcomes using AI
        /// </summary>
        [HttpPost("suggest-adjustment")]
        [ProducesResponseType(typeof(ApiResponse<TreatmentRecommendationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SuggestAdjustment([FromBody] SuggestAdjustmentRequestDto request)
        {
            try
            {
                if (request == null || request.PatientId <= 0 || request.ConditionId <= 0 || request.CurrentPlan == null)
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse("Invalid request body. Patient ID, Condition ID, and Current Plan are required."));
                }

                var recommendation = await _recommendationService.SuggestAdjustmentAsync(
                    request.PatientId, request.ConditionId, request.CurrentPlan, request.Outcomes);

                if (recommendation == null)
                {
                    return StatusCode(500, ApiResponse<object>.ErrorResponse("AI service was unable to generate adjustment suggestions. Check settings and API key."));
                }

                return Ok(ApiResponse<TreatmentRecommendationDto>.SuccessResponse(
                    recommendation,
                    "Treatment plan adjustments suggested successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error suggesting treatment adjustments");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while suggesting adjustments",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Search conditions by name or category
        /// </summary>
        [HttpGet("conditions/search")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ConditionDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchConditions([FromQuery] string searchTerm, [FromQuery] string? category = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse("Search term is required"));
                }

                var conditions = await _recommendationService.SearchConditionsAsync(searchTerm, category);
                return Ok(ApiResponse<IEnumerable<ConditionDto>>.SuccessResponse(
                    conditions,
                    "Conditions retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching conditions with term {SearchTerm}", searchTerm);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while searching conditions",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get herbal medicines with optional filters
        /// </summary>
        [HttpGet("medicines")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<HerbalMedicineDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetHerbalMedicines(
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? prakritiEffect = null)
        {
            try
            {
                var medicines = await _recommendationService.GetHerbalMedicinesAsync(searchTerm, prakritiEffect);
                return Ok(ApiResponse<IEnumerable<HerbalMedicineDto>>.SuccessResponse(
                    medicines,
                    "Herbal medicines retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving herbal medicines");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving herbal medicines",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get yoga asanas with optional filters
        /// </summary>
        [HttpGet("yoga")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<YogaAsanaDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetYogaAsanas(
            [FromQuery] string? category = null,
            [FromQuery] string? difficulty = null,
            [FromQuery] string? prakritiEffect = null)
        {
            try
            {
                var asanas = await _recommendationService.GetYogaAsanasAsync(category, difficulty, prakritiEffect);
                return Ok(ApiResponse<IEnumerable<YogaAsanaDto>>.SuccessResponse(
                    asanas,
                    "Yoga asanas retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving yoga asanas");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving yoga asanas",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get dietary items for a specific Prakriti
        /// </summary>
        [HttpGet("dietary")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DietaryItemDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDietaryItems(
            [FromQuery] string prakriti,
            [FromQuery] string? category = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(prakriti))
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse("Prakriti is required"));
                }

                var items = await _recommendationService.GetDietaryItemsAsync(prakriti, category);
                return Ok(ApiResponse<IEnumerable<DietaryItemDto>>.SuccessResponse(
                    items,
                    "Dietary items retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving dietary items for Prakriti {Prakriti}", prakriti);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving dietary items",
                    new List<string> { ex.Message }));
            }
        }
    }
}
