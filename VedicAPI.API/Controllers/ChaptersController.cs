using Microsoft.AspNetCore.Mvc;
using VedicAPI.API.Models.Common;
using VedicAPI.API.Models.DTOs;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Controllers
{
    /// <summary>
    /// API Controller for managing book chapters
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ChaptersController : ControllerBase
    {
        private readonly IBookChapterService _chapterService;
        private readonly ILogger<ChaptersController> _logger;

        public ChaptersController(IBookChapterService chapterService, ILogger<ChaptersController> logger)
        {
            _chapterService = chapterService;
            _logger = logger;
        }

        /// <summary>
        /// Get a chapter by ID
        /// </summary>
        /// <param name="id">Chapter ID</param>
        /// <returns>Chapter details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ChapterDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<ChapterDetailDto>>> GetChapter(int id)
        {
            try
            {
                var chapter = await _chapterService.GetChapterByIdAsync(id);
                
                if (chapter == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse($"Chapter with ID {id} not found"));
                }

                return Ok(ApiResponse<ChapterDetailDto>.SuccessResponse(chapter, "Chapter retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving chapter with ID {ChapterId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving the chapter"));
            }
        }

        /// <summary>
        /// Create a new chapter for a book
        /// </summary>
        /// <param name="bookId">Book ID</param>
        /// <param name="chapterDto">Chapter creation data</param>
        /// <returns>Created chapter</returns>
        [HttpPost("books/{bookId}")]
        [ProducesResponseType(typeof(ApiResponse<ChapterDetailDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<ChapterDetailDto>>> CreateChapter(
            int bookId, 
            [FromBody] ChapterCreateDto chapterDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse("Invalid chapter data", 
                        ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()));
                }

                var chapter = await _chapterService.CreateChapterAsync(bookId, chapterDto);
                
                return CreatedAtAction(
                    nameof(GetChapter), 
                    new { id = chapter.Id }, 
                    ApiResponse<ChapterDetailDto>.SuccessResponse(chapter, "Chapter created successfully"));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid argument when creating chapter for book {BookId}", bookId);
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating chapter for book {BookId}", bookId);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while creating the chapter"));
            }
        }

        /// <summary>
        /// Update an existing chapter
        /// </summary>
        /// <param name="id">Chapter ID</param>
        /// <param name="chapterDto">Chapter update data</param>
        /// <returns>Updated chapter</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ChapterDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<ChapterDetailDto>>> UpdateChapter(
            int id, 
            [FromBody] ChapterUpdateDto chapterDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse("Invalid chapter data",
                        ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()));
                }

                var chapter = await _chapterService.UpdateChapterAsync(id, chapterDto);
                
                if (chapter == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse($"Chapter with ID {id} not found"));
                }

                return Ok(ApiResponse<ChapterDetailDto>.SuccessResponse(chapter, "Chapter updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating chapter with ID {ChapterId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while updating the chapter"));
            }
        }

        /// <summary>
        /// Delete a chapter
        /// </summary>
        /// <param name="id">Chapter ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteChapter(int id)
        {
            try
            {
                var success = await _chapterService.DeleteChapterAsync(id);
                
                if (!success)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse($"Chapter with ID {id} not found"));
                }

                return Ok(ApiResponse<object>.SuccessResponse(null, "Chapter deleted successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting chapter with ID {ChapterId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while deleting the chapter"));
            }
        }
    }
}

