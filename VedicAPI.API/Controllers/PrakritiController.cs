using Microsoft.AspNetCore.Mvc;
using VedicAPI.API.Models.Common;
using VedicAPI.API.Models.DTOs;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Controllers
{
    /// <summary>
    /// Controller for Prakriti assessment operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PrakritiController : ControllerBase
    {
        private readonly IPrakritiAssessmentService _prakritiService;
        private readonly ILogger<PrakritiController> _logger;

        public PrakritiController(
            IPrakritiAssessmentService prakritiService,
            ILogger<PrakritiController> logger)
        {
            _prakritiService = prakritiService;
            _logger = logger;
        }

        /// <summary>
        /// Get all Prakriti assessment questions
        /// </summary>
        [HttpGet("questions")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PrakritiQuestionDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetQuestions()
        {
            try
            {
                var questions = await _prakritiService.GetQuestionsAsync();
                return Ok(ApiResponse<IEnumerable<PrakritiQuestionDto>>.SuccessResponse(
                    questions,
                    "Prakriti questions retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Prakriti questions");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving questions",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Submit Prakriti assessment
        /// </summary>
        [HttpPost("assess")]
        [ProducesResponseType(typeof(ApiResponse<PrakritiAssessmentResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SubmitAssessment([FromBody] PrakritiAssessmentRequestDto assessmentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse(
                        "Invalid assessment data",
                        ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()));
                }

                var assessment = await _prakritiService.SaveAssessmentAsync(assessmentDto);
                return CreatedAtAction(
                    nameof(GetAssessmentById),
                    new { id = assessment.Id },
                    ApiResponse<PrakritiAssessmentResponseDto>.SuccessResponse(
                        assessment,
                        "Prakriti assessment saved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting Prakriti assessment");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while saving the assessment",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get Prakriti assessment by ID
        /// </summary>
        [HttpGet("assessment/{id}")]
        [ProducesResponseType(typeof(ApiResponse<PrakritiAssessmentResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAssessmentById(long id)
        {
            try
            {
                var assessment = await _prakritiService.GetAssessmentByIdAsync(id);
                if (assessment == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse($"Assessment with ID {id} not found"));
                }

                return Ok(ApiResponse<PrakritiAssessmentResponseDto>.SuccessResponse(
                    assessment,
                    "Assessment retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving assessment with ID {AssessmentId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving the assessment",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get latest Prakriti assessment for a patient
        /// </summary>
        [HttpGet("patient/{patientId}/latest")]
        [ProducesResponseType(typeof(ApiResponse<PrakritiAssessmentResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLatestAssessmentByPatientId(long patientId)
        {
            try
            {
                var assessment = await _prakritiService.GetLatestAssessmentByPatientIdAsync(patientId);
                if (assessment == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse(
                        $"No assessment found for patient ID {patientId}"));
                }

                return Ok(ApiResponse<PrakritiAssessmentResponseDto>.SuccessResponse(
                    assessment,
                    "Latest assessment retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving latest assessment for patient {PatientId}", patientId);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving the assessment",
                    new List<string> { ex.Message }));
            }
        }
    }
}
