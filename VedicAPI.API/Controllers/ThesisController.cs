using Microsoft.AspNetCore.Mvc;
using VedicAPI.API.Models.Common;
using VedicAPI.API.Models.DTOs;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Controllers
{
    /// <summary>
    /// API Controller for Thesis management
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ThesisController : ControllerBase
    {
        private readonly IThesisService _service;
        private readonly ILogger<ThesisController> _logger;

        public ThesisController(
            IThesisService service,
            ILogger<ThesisController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Get all thesis
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ThesisResponseDto>>), 200)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var thesisList = await _service.GetAllThesisAsync();
                return Ok(ApiResponse<IEnumerable<ThesisResponseDto>>.SuccessResponse(
                    thesisList, "Thesis retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving thesis");
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving thesis"));
            }
        }

        /// <summary>
        /// Get featured thesis
        /// </summary>
        [HttpGet("featured")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ThesisResponseDto>>), 200)]
        public async Task<IActionResult> GetFeatured([FromQuery] int count = 3)
        {
            try
            {
                var thesisList = await _service.GetFeaturedThesisAsync(count);
                return Ok(ApiResponse<IEnumerable<ThesisResponseDto>>.SuccessResponse(
                    thesisList, "Featured thesis retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving featured thesis");
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving featured thesis"));
            }
        }

        /// <summary>
        /// Get thesis by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ThesisDetailDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var thesis = await _service.GetThesisByIdAsync(id);
                if (thesis == null)
                    return NotFound(ApiResponse<object>.ErrorResponse("Thesis not found"));

                return Ok(ApiResponse<ThesisDetailDto>.SuccessResponse(
                    thesis, "Thesis retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving thesis {Id}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving the thesis"));
            }
        }

        /// <summary>
        /// Get thesis by category
        /// </summary>
        [HttpGet("category/{category}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ThesisResponseDto>>), 200)]
        public async Task<IActionResult> GetByCategory(string category)
        {
            try
            {
                var thesisList = await _service.GetThesisByCategoryAsync(category);
                return Ok(ApiResponse<IEnumerable<ThesisResponseDto>>.SuccessResponse(
                    thesisList, $"Thesis in category '{category}' retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving thesis by category: {Category}", category);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving thesis"));
            }
        }

        /// <summary>
        /// Get thesis by year
        /// </summary>
        [HttpGet("year/{year}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ThesisResponseDto>>), 200)]
        public async Task<IActionResult> GetByYear(int year)
        {
            try
            {
                var thesisList = await _service.GetThesisByYearAsync(year);
                return Ok(ApiResponse<IEnumerable<ThesisResponseDto>>.SuccessResponse(
                    thesisList, $"Thesis from year {year} retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving thesis by year: {Year}", year);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving thesis"));
            }
        }

        /// <summary>
        /// Get thesis by institution
        /// </summary>
        [HttpGet("institution/{institution}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ThesisResponseDto>>), 200)]
        public async Task<IActionResult> GetByInstitution(string institution)
        {
            try
            {
                var thesisList = await _service.GetThesisByInstitutionAsync(institution);
                return Ok(ApiResponse<IEnumerable<ThesisResponseDto>>.SuccessResponse(
                    thesisList, $"Thesis from '{institution}' retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving thesis by institution: {Institution}", institution);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving thesis"));
            }
        }

        /// <summary>
        /// Search thesis
        /// </summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ThesisResponseDto>>), 200)]
        public async Task<IActionResult> Search([FromQuery] string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return BadRequest(ApiResponse<object>.ErrorResponse("Search term is required"));

                var thesisList = await _service.SearchThesisAsync(searchTerm);
                return Ok(ApiResponse<IEnumerable<ThesisResponseDto>>.SuccessResponse(
                    thesisList, "Search completed successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching thesis with term: {SearchTerm}", searchTerm);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while searching thesis"));
            }
        }

        /// <summary>
        /// Create a new thesis
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ThesisResponseDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> Create([FromBody] ThesisCreateDto thesisDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<object>.ErrorResponse("Invalid data"));

                var thesis = await _service.CreateThesisAsync(thesisDto);
                return CreatedAtAction(nameof(GetById), new { id = thesis.Id },
                    ApiResponse<ThesisResponseDto>.SuccessResponse(thesis, "Thesis created successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating thesis");
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while creating the thesis"));
            }
        }

        /// <summary>
        /// Update an existing thesis
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ThesisResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> Update(int id, [FromBody] ThesisUpdateDto thesisDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<object>.ErrorResponse("Invalid data"));
                    
                var thesis = await _service.UpdateThesisAsync(id, thesisDto);
                if (thesis == null)
                    return NotFound(ApiResponse<object>.ErrorResponse("Thesis not found"));

                return Ok(ApiResponse<ThesisResponseDto>.SuccessResponse(
                    thesis, "Thesis updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating thesis {Id}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while updating the thesis"));
            }
        }

        /// <summary>
        /// Delete a thesis
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.DeleteThesisAsync(id);
                if (!result)
                    return NotFound(ApiResponse<object>.ErrorResponse("Thesis not found"));

                return Ok(ApiResponse<object>.SuccessResponse(null, "Thesis deleted successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting thesis {Id}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while deleting the thesis"));
            }
        }

        /// <summary>
        /// Increment download count
        /// </summary>
        [HttpPost("{id}/download")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> IncrementDownloadCount(int id)
        {
            try
            {
                await _service.IncrementDownloadCountAsync(id);
                return Ok(ApiResponse<object>.SuccessResponse(null, "Download count incremented"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error incrementing download count for thesis {Id}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred"));
            }
        }
    }
}

