using Microsoft.AspNetCore.Mvc;
using VedicAPI.API.Models.Common;
using VedicAPI.API.Models.DTOs;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Controllers
{
    /// <summary>
    /// API Controller for managing textbooks
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TextbooksController : ControllerBase
    {
        private readonly ITextbookService _textbookService;
        private readonly ILogger<TextbooksController> _logger;

        public TextbooksController(ITextbookService textbookService, ILogger<TextbooksController> logger)
        {
            _textbookService = textbookService;
            _logger = logger;
        }

        /// <summary>
        /// Get all textbooks
        /// </summary>
        /// <returns>List of all textbooks</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TextbookResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<TextbookResponseDto>>>> GetAllTextbooks()
        {
            try
            {
                var textbooks = await _textbookService.GetAllTextbooksAsync();
                return Ok(ApiResponse<IEnumerable<TextbookResponseDto>>.SuccessResponse(
                    textbooks, 
                    "Textbooks retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all textbooks");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving textbooks",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get active textbooks only
        /// </summary>
        /// <returns>List of active textbooks</returns>
        [HttpGet("active")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TextbookResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<TextbookResponseDto>>>> GetActiveTextbooks()
        {
            try
            {
                var textbooks = await _textbookService.GetActiveTextbooksAsync();
                return Ok(ApiResponse<IEnumerable<TextbookResponseDto>>.SuccessResponse(
                    textbooks, 
                    "Active textbooks retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active textbooks");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving active textbooks",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get textbook by ID
        /// </summary>
        /// <param name="id">Textbook ID</param>
        /// <returns>Textbook details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<TextbookResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<TextbookResponseDto>>> GetTextbookById(int id)
        {
            try
            {
                var textbook = await _textbookService.GetTextbookByIdAsync(id);
                
                if (textbook == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse(
                        $"Textbook with ID {id} not found",
                        new List<string> { "Textbook not found" }));
                }

                return Ok(ApiResponse<TextbookResponseDto>.SuccessResponse(
                    textbook, 
                    "Textbook retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving textbook with ID {TextbookId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving the textbook",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get textbook with all its chapters
        /// </summary>
        /// <param name="id">Textbook ID</param>
        /// <returns>Textbook with chapters</returns>
        [HttpGet("{id}/chapters")]
        [ProducesResponseType(typeof(ApiResponse<TextbookWithChaptersDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<TextbookWithChaptersDto>>> GetTextbookWithChapters(int id)
        {
            try
            {
                var textbook = await _textbookService.GetTextbookWithChaptersAsync(id);
                
                if (textbook == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse(
                        $"Textbook with ID {id} not found",
                        new List<string> { "Textbook not found" }));
                }

                return Ok(ApiResponse<TextbookWithChaptersDto>.SuccessResponse(
                    textbook, 
                    "Textbook with chapters retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving textbook with chapters for ID {TextbookId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving the textbook with chapters",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get textbooks by category
        /// </summary>
        /// <param name="category">Category name</param>
        /// <returns>List of textbooks in the specified category</returns>
        [HttpGet("category/{category}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TextbookResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<TextbookResponseDto>>>> GetTextbooksByCategory(string category)
        {
            try
            {
                var textbooks = await _textbookService.GetTextbooksByCategoryAsync(category);
                return Ok(ApiResponse<IEnumerable<TextbookResponseDto>>.SuccessResponse(
                    textbooks, 
                    $"Textbooks in category '{category}' retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving textbooks by category {Category}", category);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving textbooks by category",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get textbooks by level (UG/PG)
        /// </summary>
        /// <param name="level">Level (UG, PG, etc.)</param>
        /// <returns>List of textbooks for the specified level</returns>
        [HttpGet("level/{level}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TextbookResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<TextbookResponseDto>>>> GetTextbooksByLevel(string level)
        {
            try
            {
                var textbooks = await _textbookService.GetTextbooksByLevelAsync(level);
                return Ok(ApiResponse<IEnumerable<TextbookResponseDto>>.SuccessResponse(
                    textbooks, 
                    $"Textbooks for level '{level}' retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving textbooks by level {Level}", level);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving textbooks by level",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Search textbooks
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        /// <returns>List of matching textbooks</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TextbookResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<TextbookResponseDto>>>> SearchTextbooks([FromQuery] string searchTerm)
        {
            try
            {
                var textbooks = await _textbookService.SearchTextbooksAsync(searchTerm);
                return Ok(ApiResponse<IEnumerable<TextbookResponseDto>>.SuccessResponse(
                    textbooks, 
                    "Search completed successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching textbooks with term {SearchTerm}", searchTerm);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while searching textbooks",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Create a new textbook
        /// </summary>
        /// <param name="textbookDto">Textbook data</param>
        /// <returns>Created textbook</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<TextbookResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<TextbookResponseDto>>> CreateTextbook([FromBody] TextbookCreateDto textbookDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                
                return BadRequest(ApiResponse<object>.ErrorResponse(
                    "Validation failed",
                    errors));
            }

            try
            {
                var createdTextbook = await _textbookService.CreateTextbookAsync(textbookDto);
                
                return CreatedAtAction(
                    nameof(GetTextbookById),
                    new { id = createdTextbook.Id },
                    ApiResponse<TextbookResponseDto>.SuccessResponse(
                        createdTextbook, 
                        "Textbook created successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating textbook");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while creating the textbook",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Update an existing textbook
        /// </summary>
        /// <param name="id">Textbook ID</param>
        /// <param name="textbookDto">Updated textbook data</param>
        /// <returns>Updated textbook</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<TextbookResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<TextbookResponseDto>>> UpdateTextbook(int id, [FromBody] TextbookUpdateDto textbookDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                
                return BadRequest(ApiResponse<object>.ErrorResponse(
                    "Validation failed",
                    errors));
            }

            try
            {
                var updatedTextbook = await _textbookService.UpdateTextbookAsync(id, textbookDto);
                
                if (updatedTextbook == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse(
                        $"Textbook with ID {id} not found",
                        new List<string> { "Textbook not found" }));
                }

                return Ok(ApiResponse<TextbookResponseDto>.SuccessResponse(
                    updatedTextbook, 
                    "Textbook updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating textbook with ID {TextbookId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while updating the textbook",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Delete a textbook
        /// </summary>
        /// <param name="id">Textbook ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteTextbook(int id)
        {
            try
            {
                var success = await _textbookService.DeleteTextbookAsync(id);
                
                if (!success)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse(
                        $"Textbook with ID {id} not found",
                        null));
                }

                return Ok(ApiResponse<object>.SuccessResponse(
                    new { }, 
                    "Textbook deleted successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting textbook with ID {TextbookId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while deleting the textbook",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Increment download count for a textbook
        /// </summary>
        /// <param name="id">Textbook ID</param>
        /// <returns>Success status</returns>
        [HttpPost("{id}/download")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> IncrementDownloadCount(int id)
        {
            try
            {
                var success = await _textbookService.IncrementDownloadCountAsync(id);
                
                if (!success)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse(
                        $"Textbook with ID {id} not found",
                        null));
                }

                return Ok(ApiResponse<object>.SuccessResponse(
                    new { }, 
                    "Download count incremented successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error incrementing download count for textbook ID {TextbookId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while incrementing download count",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Increment view count for a textbook
        /// </summary>
        /// <param name="id">Textbook ID</param>
        /// <returns>Success status</returns>
        [HttpPost("{id}/view")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> IncrementViewCount(int id)
        {
            try
            {
                var success = await _textbookService.IncrementViewCountAsync(id);
                
                if (!success)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse(
                        $"Textbook with ID {id} not found",
                        null));
                }

                return Ok(ApiResponse<object>.SuccessResponse(
                    new { }, 
                    "View count incremented successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error incrementing view count for textbook ID {TextbookId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while incrementing view count",
                    new List<string> { ex.Message }));
            }
        }
    }
}

