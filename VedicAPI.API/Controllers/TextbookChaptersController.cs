using Microsoft.AspNetCore.Mvc;
using VedicAPI.API.Models.Common;
using VedicAPI.API.Models.DTOs;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Controllers
{
    /// <summary>
    /// API Controller for managing textbook chapters
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TextbookChaptersController : ControllerBase
    {
        private readonly ITextbookChapterService _chapterService;
        private readonly ILogger<TextbookChaptersController> _logger;

        public TextbookChaptersController(ITextbookChapterService chapterService, ILogger<TextbookChaptersController> logger)
        {
            _chapterService = chapterService ?? throw new ArgumentNullException(nameof(chapterService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get a textbook chapter by ID
        /// </summary>
        /// <param name="id">Chapter ID</param>
        /// <returns>Chapter details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<TextbookChapterDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<TextbookChapterDetailDto>>> GetChapter(int id)
        {
            try
            {
                var chapter = await _chapterService.GetChapterByIdAsync(id);
                
                if (chapter == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse(
                        $"Chapter with ID {id} not found",
                        new List<string> { "Chapter not found" }));
                }

                return Ok(ApiResponse<TextbookChapterDetailDto>.SuccessResponse(
                    chapter, 
                    "Chapter retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving chapter with ID {ChapterId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving the chapter",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Create a new chapter for a textbook
        /// </summary>
        /// <param name="textbookId">Textbook ID</param>
        /// <param name="chapterDto">Chapter creation data</param>
        /// <returns>Created chapter</returns>
        [HttpPost("textbooks/{textbookId}")]
        [ProducesResponseType(typeof(ApiResponse<TextbookChapterDetailDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<TextbookChapterDetailDto>>> CreateChapter(
            int textbookId, 
            [FromBody] TextbookChapterCreateDto chapterDto)
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
                var chapter = await _chapterService.CreateChapterAsync(textbookId, chapterDto);
                
                return CreatedAtAction(
                    nameof(GetChapter), 
                    new { id = chapter.Id }, 
                    ApiResponse<TextbookChapterDetailDto>.SuccessResponse(
                        chapter, 
                        "Chapter created successfully"));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid argument when creating chapter for textbook {TextbookId}", textbookId);
                return BadRequest(ApiResponse<object>.ErrorResponse(
                    ex.Message,
                    new List<string> { ex.Message }));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation when creating chapter for textbook {TextbookId}", textbookId);
                return BadRequest(ApiResponse<object>.ErrorResponse(
                    ex.Message,
                    new List<string> { ex.Message }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating chapter for textbook {TextbookId}", textbookId);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while creating the chapter",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Update an existing textbook chapter
        /// </summary>
        /// <param name="id">Chapter ID</param>
        /// <param name="chapterDto">Chapter update data</param>
        /// <returns>Updated chapter</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<TextbookChapterDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<TextbookChapterDetailDto>>> UpdateChapter(
            int id, 
            [FromBody] TextbookChapterUpdateDto chapterDto)
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
                var chapter = await _chapterService.UpdateChapterAsync(id, chapterDto);
                
                if (chapter == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse(
                        $"Chapter with ID {id} not found",
                        new List<string> { "Chapter not found" }));
                }

                return Ok(ApiResponse<TextbookChapterDetailDto>.SuccessResponse(
                    chapter, 
                    "Chapter updated successfully"));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation when updating chapter {ChapterId}", id);
                return BadRequest(ApiResponse<object>.ErrorResponse(
                    ex.Message,
                    new List<string> { ex.Message }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating chapter with ID {ChapterId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while updating the chapter",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Delete a textbook chapter
        /// </summary>
        /// <param name="id">Chapter ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteChapter(int id)
        {
            try
            {
                var success = await _chapterService.DeleteChapterAsync(id);
                
                if (!success)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse(
                        $"Chapter with ID {id} not found",
                        null));
                }

                return Ok(ApiResponse<object>.SuccessResponse(
                    new { }, 
                    "Chapter deleted successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting chapter with ID {ChapterId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while deleting the chapter",
                    new List<string> { ex.Message }));
            }
        }
    }
}

