using Microsoft.AspNetCore.Mvc;
using VedicAPI.API.Models.Common;
using VedicAPI.API.Models.DTOs;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Controllers
{
    /// <summary>
    /// API Controller for Jurisprudence (Legal & Policy Documents) management
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class JurisprudenceController : ControllerBase
    {
        private readonly IJurisprudenceService _service;
        private readonly ILogger<JurisprudenceController> _logger;
        private readonly IWebHostEnvironment _environment;

        public JurisprudenceController(
            IJurisprudenceService service,
            ILogger<JurisprudenceController> logger,
            IWebHostEnvironment environment)
        {
            _service = service;
            _logger = logger;
            _environment = environment;
        }

        /// <summary>
        /// Get paged list of jurisprudence items (public - active only by default)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<JurisprudenceItemDto>>), 200)]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? type = null,
            [FromQuery] bool includeInactive = false)
        {
            try
            {
                // Public users cannot request inactive items
                if (!User.Identity?.IsAuthenticated ?? true)
                    includeInactive = false;

                // Cap page size
                if (pageSize > 50) pageSize = 50;
                if (pageSize < 1) pageSize = 10;
                if (page < 1) page = 1;

                var result = await _service.GetPagedAsync(
                    searchTerm,
                    type,
                    page,
                    pageSize,
                    includeInactive);

                return Ok(ApiResponse<PagedResult<JurisprudenceItemDto>>.SuccessResponse(
                    result, "Jurisprudence items retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving jurisprudence items");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving jurisprudence items"));
            }
        }

        /// <summary>
        /// Get jurisprudence item by ID (public - active only by default)
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<JurisprudenceItemDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> GetById(
            int id,
            [FromQuery] bool includeInactive = false)
        {
            try
            {
                // Public users cannot request inactive items
                if (!User.Identity?.IsAuthenticated ?? true)
                    includeInactive = false;

                var item = await _service.GetByIdAsync(id, includeInactive);
                if (item == null)
                    return NotFound(ApiResponse<object>.ErrorResponse("Jurisprudence item not found"));

                return Ok(ApiResponse<JurisprudenceItemDto>.SuccessResponse(
                    item, "Jurisprudence item retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving jurisprudence item {Id}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving the jurisprudence item"));
            }
        }

        /// <summary>
        /// Create a new jurisprudence item (admin - supports multipart file upload or JSON)
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<JurisprudenceItemDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> Create()
        {
            try
            {
                JurisprudenceCreateDto? dto = null;
                IFormFile? document = null;
                string? documentUrl = null;

                // Check if request is multipart/form-data
                if (Request.HasFormContentType)
                {
                    dto = new JurisprudenceCreateDto
                    {
                        Title = Request.Form["Title"].ToString(),
                        Type = Request.Form["Type"].ToString(),
                        Date = Request.Form["Date"].ToString(),
                        Description = Request.Form["Description"].ToString(),
                        DocumentUrl = Request.Form["DocumentUrl"].ToString(),
                        State = Request.Form["State"].ToString(),
                        DisplayOrder = int.TryParse(Request.Form["DisplayOrder"].ToString(), out var order) ? order : 0
                    };
                    document = Request.Form.Files.GetFile("document");
                }
                else
                {
                    // JSON body
                    dto = await System.Text.Json.JsonSerializer.DeserializeAsync<JurisprudenceCreateDto>(
                        Request.Body,
                        new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                if (dto == null || !ModelState.IsValid)
                    return BadRequest(ApiResponse<object>.ErrorResponse("Invalid data"));

                // Handle file upload if provided
                if (document != null && document.Length > 0)
                {
                    documentUrl = await SaveDocumentAsync(document);
                    if (documentUrl == null)
                        return BadRequest(ApiResponse<object>.ErrorResponse("Failed to save document"));
                }
                else if (dto.DocumentUrl != null)
                {
                    documentUrl = dto.DocumentUrl;
                }

                var item = await _service.CreateAsync(dto, documentUrl);
                return CreatedAtAction(nameof(GetById), new { id = item.Id },
                    ApiResponse<JurisprudenceItemDto>.SuccessResponse(item, "Jurisprudence item created successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating jurisprudence item");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while creating the jurisprudence item"));
            }
        }

        /// <summary>
        /// Update an existing jurisprudence item (admin - supports multipart file upload or JSON)
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<JurisprudenceItemDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                JurisprudenceUpdateDto? dto = null;
                IFormFile? document = null;
                string? documentUrl = null;

                // Check if request is multipart/form-data
                if (Request.HasFormContentType)
                {
                    dto = new JurisprudenceUpdateDto
                    {
                        Title = Request.Form["Title"].ToString(),
                        Type = Request.Form["Type"].ToString(),
                        Date = Request.Form["Date"].ToString(),
                        Description = Request.Form["Description"].ToString(),
                        DocumentUrl = Request.Form["DocumentUrl"].ToString(),
                        State = Request.Form["State"].ToString(),
                        DisplayOrder = int.TryParse(Request.Form["DisplayOrder"].ToString(), out var order) ? order : 0
                    };
                    document = Request.Form.Files.GetFile("document");
                }
                else
                {
                    // JSON body
                    dto = await System.Text.Json.JsonSerializer.DeserializeAsync<JurisprudenceUpdateDto>(
                        Request.Body,
                        new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                if (dto == null || !ModelState.IsValid)
                    return BadRequest(ApiResponse<object>.ErrorResponse("Invalid data"));

                // Handle file upload if provided (replaces existing)
                if (document != null && document.Length > 0)
                {
                    documentUrl = await SaveDocumentAsync(document, id);
                    if (documentUrl == null)
                        return BadRequest(ApiResponse<object>.ErrorResponse("Failed to save document"));
                }
                else if (dto.DocumentUrl != null)
                {
                    documentUrl = dto.DocumentUrl;
                }

                var item = await _service.UpdateAsync(id, dto, documentUrl);
                if (item == null)
                    return NotFound(ApiResponse<object>.ErrorResponse("Jurisprudence item not found"));

                return Ok(ApiResponse<JurisprudenceItemDto>.SuccessResponse(
                    item, "Jurisprudence item updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating jurisprudence item {Id}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while updating the jurisprudence item"));
            }
        }

        /// <summary>
        /// Delete a jurisprudence item (admin - soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);
                if (!result)
                    return NotFound(ApiResponse<object>.ErrorResponse("Jurisprudence item not found"));

                return Ok(ApiResponse<object>.SuccessResponse(null, "Jurisprudence item deleted successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting jurisprudence item {Id}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while deleting the jurisprudence item"));
            }
        }

        /// <summary>
        /// Save uploaded document file and return relative URL
        /// </summary>
        private async Task<string?> SaveDocumentAsync(IFormFile file, int? itemId = null)
        {
            try
            {
                // Validate file type (PDF, DOC, DOCX)
                var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    _logger.LogWarning("Invalid file type: {Extension}", extension);
                    return null;
                }

                // Validate file size (max 10MB)
                const long maxSize = 10 * 1024 * 1024; // 10MB
                if (file.Length > maxSize)
                {
                    _logger.LogWarning("File size exceeds limit: {Size} bytes", file.Length);
                    return null;
                }

                // Create directory if it doesn't exist
                var documentsPath = Path.Combine(_environment.WebRootPath ?? _environment.ContentRootPath, "documents", "jurisprudence");
                Directory.CreateDirectory(documentsPath);

                // Generate safe filename: {id}_{guid}{extension} or {guid}{extension}
                var fileName = itemId.HasValue
                    ? $"{itemId}_{Guid.NewGuid()}{extension}"
                    : $"{Guid.NewGuid()}{extension}";

                var filePath = Path.Combine(documentsPath, fileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Return relative URL for serving via static files
                return $"/documents/jurisprudence/{fileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving document file");
                return null;
            }
        }

    }
}
