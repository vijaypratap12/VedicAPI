using Microsoft.AspNetCore.Mvc;
using VedicAPI.API.Models.Common;
using VedicAPI.API.Models.DTOs;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Controllers
{
    /// <summary>
    /// API Controller for managing books
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IBookChapterService _chapterService;
        private readonly ILogger<BooksController> _logger;

        public BooksController(
            IBookService bookService, 
            IBookChapterService chapterService,
            ILogger<BooksController> logger)
        {
            _bookService = bookService;
            _chapterService = chapterService;
            _logger = logger;
        }

        /// <summary>
        /// Get all books
        /// </summary>
        /// <returns>List of all books</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<BookResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<BookResponseDto>>>> GetAllBooks()
        {
            try
            {
                var books = await _bookService.GetAllBooksAsync();
                return Ok(ApiResponse<IEnumerable<BookResponseDto>>.SuccessResponse(
                    books, 
                    "Books retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all books");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving books",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get active books only
        /// </summary>
        /// <returns>List of active books</returns>
        [HttpGet("active")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<BookResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<BookResponseDto>>>> GetActiveBooks()
        {
            try
            {
                var books = await _bookService.GetActiveBooksAsync();
                return Ok(ApiResponse<IEnumerable<BookResponseDto>>.SuccessResponse(
                    books, 
                    "Active books retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active books");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving active books",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get book by ID
        /// </summary>
        /// <param name="id">Book ID</param>
        /// <returns>Book details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<BookResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<BookResponseDto>>> GetBookById(int id)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(id);
                if (book == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse(
                        $"Book with ID {id} not found"));
                }

                return Ok(ApiResponse<BookResponseDto>.SuccessResponse(
                    book, 
                    "Book retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving book with ID {BookId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving the book",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get books by category
        /// </summary>
        /// <param name="category">Category name</param>
        /// <returns>List of books in the category</returns>
        [HttpGet("category/{category}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<BookResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<BookResponseDto>>>> GetBooksByCategory(string category)
        {
            try
            {
                var books = await _bookService.GetBooksByCategoryAsync(category);
                return Ok(ApiResponse<IEnumerable<BookResponseDto>>.SuccessResponse(
                    books, 
                    $"Books in category '{category}' retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving books by category {Category}", category);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving books by category",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Search books by title, author, description, or category
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        /// <returns>List of matching books</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<BookResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<BookResponseDto>>>> SearchBooks([FromQuery] string searchTerm)
        {
            try
            {
                var books = await _bookService.SearchBooksAsync(searchTerm);
                return Ok(ApiResponse<IEnumerable<BookResponseDto>>.SuccessResponse(
                    books, 
                    "Search completed successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching books with term {SearchTerm}", searchTerm);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while searching books",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Create a new book
        /// </summary>
        /// <param name="bookDto">Book creation data</param>
        /// <returns>Created book</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<BookResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<BookResponseDto>>> CreateBook([FromBody] BookCreateDto bookDto)
        {
            try
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

                var createdBook = await _bookService.CreateBookAsync(bookDto);
                return CreatedAtAction(
                    nameof(GetBookById), 
                    new { id = createdBook.Id }, 
                    ApiResponse<BookResponseDto>.SuccessResponse(
                        createdBook, 
                        "Book created successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating book");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while creating the book",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Update an existing book
        /// </summary>
        /// <param name="id">Book ID</param>
        /// <param name="bookDto">Book update data</param>
        /// <returns>Updated book</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<BookResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<BookResponseDto>>> UpdateBook(int id, [FromBody] BookUpdateDto bookDto)
        {
            try
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

                var updatedBook = await _bookService.UpdateBookAsync(id, bookDto);
                if (updatedBook == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse(
                        $"Book with ID {id} not found"));
                }

                return Ok(ApiResponse<BookResponseDto>.SuccessResponse(
                    updatedBook, 
                    "Book updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating book with ID {BookId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while updating the book",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Delete a book
        /// </summary>
        /// <param name="id">Book ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteBook(int id)
        {
            try
            {
                var success = await _bookService.DeleteBookAsync(id);
                if (!success)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse(
                        $"Book with ID {id} not found"));
                }

                return Ok(ApiResponse<object>.SuccessResponse(
                    null, 
                    "Book deleted successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting book with ID {BookId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while deleting the book",
                    new List<string> { ex.Message }));
            }
        }

        #region Chapter Endpoints

        /// <summary>
        /// Get book with all chapters (summaries only)
        /// </summary>
        /// <param name="id">Book ID</param>
        /// <returns>Book with chapter summaries</returns>
        [HttpGet("{id}/chapters")]
        [ProducesResponseType(typeof(ApiResponse<BookWithChaptersDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<BookWithChaptersDto>>> GetBookWithChapters(int id)
        {
            try
            {
                var bookWithChapters = await _chapterService.GetBookWithChaptersAsync(id);
                if (bookWithChapters == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse($"Book with ID {id} not found"));
                }

                return Ok(ApiResponse<BookWithChaptersDto>.SuccessResponse(
                    bookWithChapters, 
                    "Book with chapters retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving book with chapters for ID {BookId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving the book with chapters"));
            }
        }

        /// <summary>
        /// Get specific chapter by chapter number
        /// </summary>
        /// <param name="bookId">Book ID</param>
        /// <param name="chapterNumber">Chapter number</param>
        /// <returns>Chapter details with content</returns>
        [HttpGet("{bookId}/chapters/{chapterNumber}")]
        [ProducesResponseType(typeof(ApiResponse<ChapterDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<ChapterDetailDto>>> GetChapterByNumber(
            int bookId, 
            int chapterNumber)
        {
            try
            {
                var chapter = await _chapterService.GetChapterByNumberAsync(bookId, chapterNumber);
                if (chapter == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse(
                        $"Chapter {chapterNumber} not found for book {bookId}"));
                }

                return Ok(ApiResponse<ChapterDetailDto>.SuccessResponse(
                    chapter, 
                    "Chapter retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving chapter {ChapterNumber} for book {BookId}", 
                    chapterNumber, bookId);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving the chapter"));
            }
        }

        /// <summary>
        /// Get next chapter
        /// </summary>
        /// <param name="bookId">Book ID</param>
        /// <param name="chapterNumber">Current chapter number</param>
        /// <returns>Next chapter details</returns>
        [HttpGet("{bookId}/chapters/{chapterNumber}/next")]
        [ProducesResponseType(typeof(ApiResponse<ChapterDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<ChapterDetailDto>>> GetNextChapter(
            int bookId, 
            int chapterNumber)
        {
            try
            {
                var chapter = await _chapterService.GetNextChapterAsync(bookId, chapterNumber);
                if (chapter == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse(
                        $"No next chapter found after chapter {chapterNumber} for book {bookId}"));
                }

                return Ok(ApiResponse<ChapterDetailDto>.SuccessResponse(
                    chapter, 
                    "Next chapter retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving next chapter after {ChapterNumber} for book {BookId}",
                    chapterNumber, bookId);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving the next chapter"));
            }
        }

        /// <summary>
        /// Get previous chapter
        /// </summary>
        /// <param name="bookId">Book ID</param>
        /// <param name="chapterNumber">Current chapter number</param>
        /// <returns>Previous chapter details</returns>
        [HttpGet("{bookId}/chapters/{chapterNumber}/previous")]
        [ProducesResponseType(typeof(ApiResponse<ChapterDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<ChapterDetailDto>>> GetPreviousChapter(
            int bookId, 
            int chapterNumber)
        {
            try
            {
                var chapter = await _chapterService.GetPreviousChapterAsync(bookId, chapterNumber);
                if (chapter == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse(
                        $"No previous chapter found before chapter {chapterNumber} for book {bookId}"));
                }

                return Ok(ApiResponse<ChapterDetailDto>.SuccessResponse(
                    chapter, 
                    "Previous chapter retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving previous chapter before {ChapterNumber} for book {BookId}",
                    chapterNumber, bookId);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving the previous chapter"));
            }
        }

        #endregion
    }
}

