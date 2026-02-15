using Microsoft.AspNetCore.Mvc;
using VedicAPI.API.Models.Common;
using VedicAPI.API.Models.DTOs;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Controllers
{
    /// <summary>
    /// Controller for Patient management operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly ILogger<PatientsController> _logger;

        public PatientsController(IPatientService patientService, ILogger<PatientsController> logger)
        {
            _patientService = patientService;
            _logger = logger;
        }

        /// <summary>
        /// Get all patients
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PatientResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllPatients()
        {
            try
            {
                var patients = await _patientService.GetAllPatientsAsync();
                return Ok(ApiResponse<IEnumerable<PatientResponseDto>>.SuccessResponse(
                    patients,
                    "Patients retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all patients");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving patients",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get patient by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<PatientResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPatientById(long id)
        {
            try
            {
                var patient = await _patientService.GetPatientByIdAsync(id);
                if (patient == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse($"Patient with ID {id} not found"));
                }

                return Ok(ApiResponse<PatientResponseDto>.SuccessResponse(
                    patient,
                    "Patient retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving patient with ID {PatientId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving the patient",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get patient by user ID
        /// </summary>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(ApiResponse<PatientResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPatientByUserId(int userId)
        {
            try
            {
                var patient = await _patientService.GetPatientByUserIdAsync(userId);
                if (patient == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse($"Patient with User ID {userId} not found"));
                }

                return Ok(ApiResponse<PatientResponseDto>.SuccessResponse(
                    patient,
                    "Patient retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving patient with User ID {UserId}", userId);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving the patient",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Create a new patient
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PatientResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePatient([FromBody] PatientCreateDto patientDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse(
                        "Invalid patient data",
                        ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()));
                }

                var patient = await _patientService.CreatePatientAsync(patientDto);
                return CreatedAtAction(
                    nameof(GetPatientById),
                    new { id = patient.Id },
                    ApiResponse<PatientResponseDto>.SuccessResponse(
                        patient,
                        "Patient created successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating patient");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while creating the patient",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Update an existing patient
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<PatientResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePatient(long id, [FromBody] PatientUpdateDto patientDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse(
                        "Invalid patient data",
                        ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()));
                }

                var patient = await _patientService.UpdatePatientAsync(id, patientDto);
                if (patient == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse($"Patient with ID {id} not found"));
                }

                return Ok(ApiResponse<PatientResponseDto>.SuccessResponse(
                    patient,
                    "Patient updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating patient with ID {PatientId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while updating the patient",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Delete a patient (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePatient(long id)
        {
            try
            {
                var success = await _patientService.DeletePatientAsync(id);
                if (!success)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse($"Patient with ID {id} not found"));
                }

                return Ok(ApiResponse<object>.SuccessResponse(
                    null,
                    "Patient deleted successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting patient with ID {PatientId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while deleting the patient",
                    new List<string> { ex.Message }));
            }
        }
    }
}
