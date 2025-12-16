using VedicAPI.API.Models;
using VedicAPI.API.Models.DTOs;
using VedicAPI.API.Repositories.Interfaces;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Services
{
    public class NewsletterService : INewsletterService
    {
        private readonly INewsletterRepository _newsletterRepository;
        private readonly ILogger<NewsletterService> _logger;

        public NewsletterService(INewsletterRepository newsletterRepository, ILogger<NewsletterService> logger)
        {
            _newsletterRepository = newsletterRepository;
            _logger = logger;
        }

        public async Task<NewsletterSubscriptionResponseDto> SubscribeAsync(NewsletterSubscribeDto dto, string? ipAddress)
        {
            try
            {
                // Check if email already exists
                var existing = await _newsletterRepository.GetByEmailAsync(dto.Email);
                
                if (existing != null)
                {
                    if (existing.IsActive)
                    {
                        _logger.LogInformation("Email already subscribed: {Email}", dto.Email);
                        return new NewsletterSubscriptionResponseDto
                        {
                            Id = existing.Id,
                            Email = existing.Email,
                            SubscribedAt = existing.SubscribedAt,
                            IsActive = existing.IsActive,
                            Source = existing.Source,
                            Message = "This email is already subscribed to our newsletter."
                        };
                    }
                    else
                    {
                        // Reactivate subscription
                        existing.IsActive = true;
                        existing.SubscribedAt = DateTime.UtcNow;
                        existing.UnsubscribedAt = null;
                        existing.UpdatedAt = DateTime.UtcNow;
                        
                        var reactivated = await _newsletterRepository.UpdateAsync(existing);
                        _logger.LogInformation("Newsletter subscription reactivated for email: {Email}", dto.Email);

                        return new NewsletterSubscriptionResponseDto
                        {
                            Id = reactivated.Id,
                            Email = reactivated.Email,
                            SubscribedAt = reactivated.SubscribedAt,
                            IsActive = reactivated.IsActive,
                            Source = reactivated.Source,
                            Message = "Successfully resubscribed to our newsletter!"
                        };
                    }
                }

                // Create new subscription
                var subscription = new NewsletterSubscription
                {
                    Id = Guid.NewGuid(),
                    Email = dto.Email,
                    Source = dto.Source,
                    IpAddress = ipAddress,
                    SubscribedAt = DateTime.UtcNow,
                    IsActive = true,
                    UnsubscribeToken = Guid.NewGuid(),
                    ConfirmationSent = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var created = await _newsletterRepository.CreateAsync(subscription);
                _logger.LogInformation("Newsletter subscription created for email: {Email}", dto.Email);

                // TODO: Send welcome email with unsubscribe link

                return new NewsletterSubscriptionResponseDto
                {
                    Id = created.Id,
                    Email = created.Email,
                    SubscribedAt = created.SubscribedAt,
                    IsActive = created.IsActive,
                    Source = created.Source,
                    Message = "Successfully subscribed to our newsletter!"
                };
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Duplicate subscription attempt for email: {Email}", dto.Email);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating newsletter subscription");
                throw;
            }
        }

        public async Task<bool> UnsubscribeAsync(Guid unsubscribeToken)
        {
            try
            {
                var result = await _newsletterRepository.UnsubscribeAsync(unsubscribeToken);
                
                if (result)
                {
                    _logger.LogInformation("Newsletter unsubscribed successfully using token: {Token}", unsubscribeToken);
                }
                else
                {
                    _logger.LogWarning("Unsubscribe token not found or already unsubscribed: {Token}", unsubscribeToken);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unsubscribing newsletter with token: {Token}", unsubscribeToken);
                throw;
            }
        }

        public async Task<NewsletterSubscriptionResponseDto?> GetSubscriptionByEmailAsync(string email)
        {
            try
            {
                var subscription = await _newsletterRepository.GetByEmailAsync(email);
                
                if (subscription == null)
                {
                    return null;
                }

                return new NewsletterSubscriptionResponseDto
                {
                    Id = subscription.Id,
                    Email = subscription.Email,
                    SubscribedAt = subscription.SubscribedAt,
                    IsActive = subscription.IsActive,
                    Source = subscription.Source,
                    Message = subscription.IsActive ? "Active subscription" : "Unsubscribed"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving newsletter subscription by email: {Email}", email);
                throw;
            }
        }

        public async Task<IEnumerable<NewsletterSubscriptionResponseDto>> GetAllActiveSubscriptionsAsync()
        {
            try
            {
                var subscriptions = await _newsletterRepository.GetAllActiveAsync();
                
                return subscriptions.Select(s => new NewsletterSubscriptionResponseDto
                {
                    Id = s.Id,
                    Email = s.Email,
                    SubscribedAt = s.SubscribedAt,
                    IsActive = s.IsActive,
                    Source = s.Source,
                    Message = "Active subscription"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active newsletter subscriptions");
                throw;
            }
        }

        public async Task<int> GetActiveSubscriptionCountAsync()
        {
            try
            {
                return await _newsletterRepository.GetActiveCountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active subscription count");
                throw;
            }
        }

        public async Task<bool> IsSubscribedAsync(string email)
        {
            try
            {
                var subscription = await _newsletterRepository.GetByEmailAsync(email);
                return subscription != null && subscription.IsActive;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if email is subscribed: {Email}", email);
                throw;
            }
        }
    }
}

