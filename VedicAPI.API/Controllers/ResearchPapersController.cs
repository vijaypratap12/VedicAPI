using Microsoft.AspNetCore.Mvc;
using VedicAPI.API.Models.Common;
using VedicAPI.API.Models.DTOs;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Controllers
{
    /// <summary>
    /// API Controller for Research Papers management
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ResearchPapersController : ControllerBase
    {
        private readonly IResearchPaperService _service;
        private readonly ILogger<ResearchPapersController> _logger;

        public ResearchPapersController(
            IResearchPaperService service,
            ILogger<ResearchPapersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Get all research papers
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ResearchPaperResponseDto>>), 200)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var papers = await _service.GetAllResearchPapersAsync();
                return Ok(ApiResponse<IEnumerable<ResearchPaperResponseDto>>.SuccessResponse(
                    papers, "Research papers retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving research papers");
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving research papers"));
            }
        }

        /// <summary>
        /// Get featured research papers
        /// </summary>
        [HttpGet("featured")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ResearchPaperResponseDto>>), 200)]
        public async Task<IActionResult> GetFeatured([FromQuery] int count = 3)
        {
            try
            {
                var papers = await _service.GetFeaturedResearchPapersAsync(count);
                return Ok(ApiResponse<IEnumerable<ResearchPaperResponseDto>>.SuccessResponse(
                    papers, "Featured research papers retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving featured research papers");
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving featured research papers"));
            }
        }

        /// <summary>
        /// Get research paper by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ResearchPaperDetailDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var paper = await _service.GetResearchPaperByIdAsync(id);
                if (paper == null)
                    return NotFound(ApiResponse<object>.ErrorResponse("Research paper not found"));

                return Ok(ApiResponse<ResearchPaperDetailDto>.SuccessResponse(
                    paper, "Research paper retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving research paper {Id}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving the research paper"));
            }
        }

        /// <summary>
        /// Get research papers by category
        /// </summary>
        [HttpGet("category/{category}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ResearchPaperResponseDto>>), 200)]
        public async Task<IActionResult> GetByCategory(string category)
        {
            try
            {
                var papers = await _service.GetResearchPapersByCategoryAsync(category);
                return Ok(ApiResponse<IEnumerable<ResearchPaperResponseDto>>.SuccessResponse(
                    papers, $"Research papers in category '{category}' retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving research papers by category: {Category}", category);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving research papers"));
            }
        }

        /// <summary>
        /// Get research papers by year
        /// </summary>
        [HttpGet("year/{year}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ResearchPaperResponseDto>>), 200)]
        public async Task<IActionResult> GetByYear(int year)
        {
            try
            {
                var papers = await _service.GetResearchPapersByYearAsync(year);
                return Ok(ApiResponse<IEnumerable<ResearchPaperResponseDto>>.SuccessResponse(
                    papers, $"Research papers from year {year} retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving research papers by year: {Year}", year);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving research papers"));
            }
        }

        /// <summary>
        /// Get research papers by institution
        /// </summary>
        [HttpGet("institution/{institution}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ResearchPaperResponseDto>>), 200)]
        public async Task<IActionResult> GetByInstitution(string institution)
        {
            try
            {
                var papers = await _service.GetResearchPapersByInstitutionAsync(institution);
                return Ok(ApiResponse<IEnumerable<ResearchPaperResponseDto>>.SuccessResponse(
                    papers, $"Research papers from '{institution}' retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving research papers by institution: {Institution}", institution);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving research papers"));
            }
        }

        /// <summary>
        /// Search research papers
        /// </summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ResearchPaperResponseDto>>), 200)]
        public async Task<IActionResult> Search([FromQuery] string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return BadRequest(ApiResponse<object>.ErrorResponse("Search term is required"));

                var papers = await _service.SearchResearchPapersAsync(searchTerm);
                return Ok(ApiResponse<IEnumerable<ResearchPaperResponseDto>>.SuccessResponse(
                    papers, "Search completed successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching research papers with term: {SearchTerm}", searchTerm);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while searching research papers"));
            }
        }

        /// <summary>
        /// Create a new research paper
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ResearchPaperResponseDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> Create([FromBody] ResearchPaperCreateDto paperDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<object>.ErrorResponse("Invalid data"));

                var paper = await _service.CreateResearchPaperAsync(paperDto);
                return CreatedAtAction(nameof(GetById), new { id = paper.Id },
                    ApiResponse<ResearchPaperResponseDto>.SuccessResponse(paper, "Research paper created successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating research paper");
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while creating the research paper"));
            }
        }

        /// <summary>
        /// Update an existing research paper
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ResearchPaperResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> Update(int id, [FromBody] ResearchPaperUpdateDto paperDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<object>.ErrorResponse("Invalid data"));

                var paper = await _service.UpdateResearchPaperAsync(id, paperDto);
                if (paper == null)
                    return NotFound(ApiResponse<object>.ErrorResponse("Research paper not found"));

                return Ok(ApiResponse<ResearchPaperResponseDto>.SuccessResponse(
                    paper, "Research paper updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating research paper {Id}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while updating the research paper"));
            }
        }

        /// <summary>
        /// Delete a research paper
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.DeleteResearchPaperAsync(id);
                if (!result)
                    return NotFound(ApiResponse<object>.ErrorResponse("Research paper not found"));

                return Ok(ApiResponse<object>.SuccessResponse(null, "Research paper deleted successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting research paper {Id}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while deleting the research paper"));
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
                _logger.LogError(ex, "Error incrementing download count for research paper {Id}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred"));
            }
        }
    }
}

