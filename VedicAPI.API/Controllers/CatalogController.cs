using Microsoft.AspNetCore.Mvc;
using VedicAPI.API.Models.Common;
using VedicAPI.API.Models.DTOs;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Controllers
{
    /// <summary>
    /// Controller for catalog operations (medicines, yoga, dietary items, statistics)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly ITreatmentRecommendationService _recommendationService;
        private readonly ITreatmentStatisticsService _statisticsService;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(
            ITreatmentRecommendationService recommendationService,
            ITreatmentStatisticsService statisticsService,
            ILogger<CatalogController> logger)
        {
            _recommendationService = recommendationService;
            _statisticsService = statisticsService;
            _logger = logger;
        }

        /// <summary>
        /// Get all herbal medicines
        /// </summary>
        [HttpGet("medicines")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<HerbalMedicineDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMedicines([FromQuery] string? searchTerm = null)
        {
            try
            {
                var medicines = await _recommendationService.GetHerbalMedicinesAsync(searchTerm);
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
        /// Get herbal medicine by ID
        /// </summary>
        [HttpGet("medicines/{id}")]
        [ProducesResponseType(typeof(ApiResponse<HerbalMedicineDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMedicineById(long id)
        {
            try
            {
                var medicines = await _recommendationService.GetHerbalMedicinesAsync();
                var medicine = medicines.FirstOrDefault(m => m.Id == id);

                if (medicine == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse($"Medicine with ID {id} not found"));
                }

                return Ok(ApiResponse<HerbalMedicineDto>.SuccessResponse(
                    medicine,
                    "Medicine retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving medicine with ID {MedicineId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving the medicine",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get all yoga asanas
        /// </summary>
        [HttpGet("yoga")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<YogaAsanaDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetYogaAsanas(
            [FromQuery] string? category = null,
            [FromQuery] string? difficulty = null)
        {
            try
            {
                var asanas = await _recommendationService.GetYogaAsanasAsync(category, difficulty);
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
        /// Get yoga asana by ID
        /// </summary>
        [HttpGet("yoga/{id}")]
        [ProducesResponseType(typeof(ApiResponse<YogaAsanaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetYogaAsanaById(long id)
        {
            try
            {
                var asanas = await _recommendationService.GetYogaAsanasAsync();
                var asana = asanas.FirstOrDefault(a => a.Id == id);

                if (asana == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse($"Yoga asana with ID {id} not found"));
                }

                return Ok(ApiResponse<YogaAsanaDto>.SuccessResponse(
                    asana,
                    "Yoga asana retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving yoga asana with ID {AsanaId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving the yoga asana",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get dietary items by Prakriti
        /// </summary>
        [HttpGet("dietary")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DietaryItemDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
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
                _logger.LogError(ex, "Error retrieving dietary items");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving dietary items",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get all conditions
        /// </summary>
        [HttpGet("conditions")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ConditionDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetConditions([FromQuery] string? searchTerm = null, [FromQuery] string? category = null)
        {
            try
            {
                var conditions = string.IsNullOrWhiteSpace(searchTerm)
                    ? await _recommendationService.SearchConditionsAsync("", category)
                    : await _recommendationService.SearchConditionsAsync(searchTerm, category);

                return Ok(ApiResponse<IEnumerable<ConditionDto>>.SuccessResponse(
                    conditions,
                    "Conditions retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving conditions");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving conditions",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get treatment system statistics
        /// </summary>
        [HttpGet("statistics")]
        [ProducesResponseType(typeof(ApiResponse<TreatmentStatisticsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStatistics()
        {
            try
            {
                var statistics = await _statisticsService.GetStatisticsAsync();
                return Ok(ApiResponse<TreatmentStatisticsDto>.SuccessResponse(
                    statistics,
                    "Statistics retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving statistics");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving statistics",
                    new List<string> { ex.Message }));
            }
        }
    }
}
