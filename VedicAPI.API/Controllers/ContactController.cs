using Microsoft.AspNetCore.Mvc;
using VedicAPI.API.Models.Common;
using VedicAPI.API.Models.DTOs;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly ILogger<ContactController> _logger;

        public ContactController(IContactService contactService, ILogger<ContactController> logger)
        {
            _contactService = contactService;
            _logger = logger;
        }

        /// <summary>
        /// Submit a new contact form
        /// </summary>
        [HttpPost("submit")]
        [ProducesResponseType(typeof(ApiResponse<ContactSubmissionResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SubmitContactForm([FromBody] ContactSubmissionCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse("Invalid data provided"));
                }

                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                var userAgent = Request.Headers["User-Agent"].ToString();

                var result = await _contactService.CreateContactSubmissionAsync(dto, ipAddress, userAgent);

                return CreatedAtAction(
                    nameof(GetContactSubmissionById),
                    new { id = result.Id },
                    ApiResponse<ContactSubmissionResponseDto>.SuccessResponse(
                        result,
                        "Your message has been sent successfully. We'll get back to you within 24-48 hours."
                    )
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting contact form");
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while submitting your message. Please try again later."));
            }
        }

        /// <summary>
        /// Get contact submission by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ContactSubmissionResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetContactSubmissionById(Guid id)
        {
            try
            {
                var result = await _contactService.GetContactSubmissionByIdAsync(id);

                if (result == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse($"Contact submission with ID {id} not found"));
                }

                return Ok(ApiResponse<ContactSubmissionResponseDto>.SuccessResponse(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving contact submission with ID: {Id}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving the contact submission"));
            }
        }

        /// <summary>
        /// Get all contact submissions
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ContactSubmissionResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllContactSubmissions()
        {
            try
            {
                var result = await _contactService.GetAllContactSubmissionsAsync();
                return Ok(ApiResponse<IEnumerable<ContactSubmissionResponseDto>>.SuccessResponse(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all contact submissions");
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving contact submissions"));
            }
        }

        /// <summary>
        /// Get contact submissions by status
        /// </summary>
        [HttpGet("status/{status}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ContactSubmissionResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetContactSubmissionsByStatus(string status)
        {
            try
            {
                var result = await _contactService.GetContactSubmissionsByStatusAsync(status);
                return Ok(ApiResponse<IEnumerable<ContactSubmissionResponseDto>>.SuccessResponse(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving contact submissions by status: {Status}", status);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving contact submissions"));
            }
        }

        /// <summary>
        /// Get contact submissions by type
        /// </summary>
        [HttpGet("type/{contactType}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ContactSubmissionResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetContactSubmissionsByType(string contactType)
        {
            try
            {
                var result = await _contactService.GetContactSubmissionsByTypeAsync(contactType);
                return Ok(ApiResponse<IEnumerable<ContactSubmissionResponseDto>>.SuccessResponse(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving contact submissions by type: {ContactType}", contactType);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving contact submissions"));
            }
        }

        /// <summary>
        /// Get contact submissions by email
        /// </summary>
        [HttpGet("email/{email}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ContactSubmissionResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetContactSubmissionsByEmail(string email)
        {
            try
            {
                var result = await _contactService.GetContactSubmissionsByEmailAsync(email);
                return Ok(ApiResponse<IEnumerable<ContactSubmissionResponseDto>>.SuccessResponse(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving contact submissions by email: {Email}", email);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving contact submissions"));
            }
        }

        /// <summary>
        /// Update contact submission
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ContactSubmissionResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateContactSubmission(Guid id, [FromBody] ContactSubmissionResponseDto dto)
        {
            try
            {
                var result = await _contactService.UpdateContactSubmissionAsync(id, dto);
                return Ok(ApiResponse<ContactSubmissionResponseDto>.SuccessResponse(result, "Contact submission updated successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating contact submission with ID: {Id}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while updating the contact submission"));
            }
        }

        /// <summary>
        /// Delete contact submission
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteContactSubmission(Guid id)
        {
            try
            {
                var result = await _contactService.DeleteContactSubmissionAsync(id);

                if (!result)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse($"Contact submission with ID {id} not found"));
                }

                return Ok(ApiResponse<object>.SuccessResponse(null, "Contact submission deleted successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting contact submission with ID: {Id}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while deleting the contact submission"));
            }
        }

        /// <summary>
        /// Get contact submission statistics
        /// </summary>
        [HttpGet("stats")]
        [ProducesResponseType(typeof(ApiResponse<Dictionary<string, int>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetContactSubmissionStats()
        {
            try
            {
                var result = await _contactService.GetContactSubmissionStatsAsync();
                return Ok(ApiResponse<Dictionary<string, int>>.SuccessResponse(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving contact submission stats");
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving statistics"));
            }
        }
    }
}

