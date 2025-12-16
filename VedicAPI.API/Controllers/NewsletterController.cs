using Microsoft.AspNetCore.Mvc;
using VedicAPI.API.Models.Common;
using VedicAPI.API.Models.DTOs;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsletterController : ControllerBase
    {
        private readonly INewsletterService _newsletterService;
        private readonly ILogger<NewsletterController> _logger;

        public NewsletterController(INewsletterService newsletterService, ILogger<NewsletterController> logger)
        {
            _newsletterService = newsletterService;
            _logger = logger;
        }

        /// <summary>
        /// Subscribe to newsletter
        /// </summary>
        [HttpPost("subscribe")]
        [ProducesResponseType(typeof(ApiResponse<NewsletterSubscriptionResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Subscribe([FromBody] NewsletterSubscribeDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse("Invalid data provided"));
                }

                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                var result = await _newsletterService.SubscribeAsync(dto, ipAddress);

                return CreatedAtAction(
                    nameof(GetSubscriptionByEmail),
                    new { email = result.Email },
                    ApiResponse<NewsletterSubscriptionResponseDto>.SuccessResponse(result, result.Message)
                );
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error subscribing to newsletter");
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while subscribing to the newsletter. Please try again later."));
            }
        }

        /// <summary>
        /// Unsubscribe from newsletter using unsubscribe token
        /// </summary>
        [HttpPost("unsubscribe/{token}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Unsubscribe(Guid token)
        {
            try
            {
                var result = await _newsletterService.UnsubscribeAsync(token);

                if (!result)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse("Unsubscribe token not found or already unsubscribed"));
                }

                return Ok(ApiResponse<object>.SuccessResponse(null, "Successfully unsubscribed from newsletter"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unsubscribing from newsletter");
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while unsubscribing from the newsletter"));
            }
        }

        /// <summary>
        /// Get subscription by email
        /// </summary>
        [HttpGet("email/{email}")]
        [ProducesResponseType(typeof(ApiResponse<NewsletterSubscriptionResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSubscriptionByEmail(string email)
        {
            try
            {
                var result = await _newsletterService.GetSubscriptionByEmailAsync(email);

                if (result == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse($"No subscription found for email: {email}"));
                }

                return Ok(ApiResponse<NewsletterSubscriptionResponseDto>.SuccessResponse(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving newsletter subscription by email: {Email}", email);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving the subscription"));
            }
        }

        /// <summary>
        /// Get all active subscriptions
        /// </summary>
        [HttpGet("active")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<NewsletterSubscriptionResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllActiveSubscriptions()
        {
            try
            {
                var result = await _newsletterService.GetAllActiveSubscriptionsAsync();
                return Ok(ApiResponse<IEnumerable<NewsletterSubscriptionResponseDto>>.SuccessResponse(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active newsletter subscriptions");
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving subscriptions"));
            }
        }

        /// <summary>
        /// Get active subscription count
        /// </summary>
        [HttpGet("count")]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActiveSubscriptionCount()
        {
            try
            {
                var count = await _newsletterService.GetActiveSubscriptionCountAsync();
                return Ok(ApiResponse<int>.SuccessResponse(count, $"Total active subscriptions: {count}"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active subscription count");
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving the count"));
            }
        }

        /// <summary>
        /// Check if email is subscribed
        /// </summary>
        [HttpGet("check/{email}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> IsSubscribed(string email)
        {
            try
            {
                var isSubscribed = await _newsletterService.IsSubscribedAsync(email);
                var message = isSubscribed ? "Email is subscribed" : "Email is not subscribed";
                return Ok(ApiResponse<bool>.SuccessResponse(isSubscribed, message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking subscription status for email: {Email}", email);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while checking subscription status"));
            }
        }
    }
}

