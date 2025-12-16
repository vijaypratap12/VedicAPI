using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VedicAPI.API.Models;

namespace VedicAPI.API.Repositories.Interfaces
{
    public interface IContactRepository
    {
        Task<ContactSubmission> CreateAsync(ContactSubmission contactSubmission);
        Task<ContactSubmission?> GetByIdAsync(Guid id);
        Task<IEnumerable<ContactSubmission>> GetAllAsync();
        Task<IEnumerable<ContactSubmission>> GetByStatusAsync(string status);
        Task<IEnumerable<ContactSubmission>> GetByContactTypeAsync(string contactType);
        Task<IEnumerable<ContactSubmission>> GetByEmailAsync(string email);
        Task<ContactSubmission> UpdateAsync(ContactSubmission contactSubmission);
        Task<bool> DeleteAsync(Guid id);
        Task<int> GetCountByStatusAsync(string status);
        Task<IEnumerable<ContactSubmission>> GetRecentAsync(int count);
    }
}

