using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VedicAPI.API.Models.DTOs;

namespace VedicAPI.API.Services.Interfaces
{
    public interface INewsletterService
    {
        Task<NewsletterSubscriptionResponseDto> SubscribeAsync(NewsletterSubscribeDto dto, string? ipAddress);
        Task<bool> UnsubscribeAsync(Guid unsubscribeToken);
        Task<NewsletterSubscriptionResponseDto?> GetSubscriptionByEmailAsync(string email);
        Task<IEnumerable<NewsletterSubscriptionResponseDto>> GetAllActiveSubscriptionsAsync();
        Task<int> GetActiveSubscriptionCountAsync();
        Task<bool> IsSubscribedAsync(string email);
    }
}

