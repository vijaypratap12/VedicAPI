using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VedicAPI.API.Models;

namespace VedicAPI.API.Repositories.Interfaces
{
    public interface INewsletterRepository
    {
        Task<NewsletterSubscription> CreateAsync(NewsletterSubscription subscription);
        Task<NewsletterSubscription?> GetByIdAsync(Guid id);
        Task<NewsletterSubscription?> GetByEmailAsync(string email);
        Task<IEnumerable<NewsletterSubscription>> GetAllActiveAsync();
        Task<IEnumerable<NewsletterSubscription>> GetAllAsync();
        Task<NewsletterSubscription> UpdateAsync(NewsletterSubscription subscription);
        Task<bool> UnsubscribeAsync(Guid unsubscribeToken);
        Task<bool> ExistsAsync(string email);
        Task<int> GetActiveCountAsync();
        Task<IEnumerable<NewsletterSubscription>> GetRecentAsync(int count);
    }
}

