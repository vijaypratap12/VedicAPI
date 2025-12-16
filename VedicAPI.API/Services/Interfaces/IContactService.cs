using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VedicAPI.API.Models.DTOs;

namespace VedicAPI.API.Services.Interfaces
{
    public interface IContactService
    {
        Task<ContactSubmissionResponseDto> CreateContactSubmissionAsync(ContactSubmissionCreateDto dto, string? ipAddress, string? userAgent);
        Task<ContactSubmissionResponseDto?> GetContactSubmissionByIdAsync(Guid id);
        Task<IEnumerable<ContactSubmissionResponseDto>> GetAllContactSubmissionsAsync();
        Task<IEnumerable<ContactSubmissionResponseDto>> GetContactSubmissionsByStatusAsync(string status);
        Task<IEnumerable<ContactSubmissionResponseDto>> GetContactSubmissionsByTypeAsync(string contactType);
        Task<IEnumerable<ContactSubmissionResponseDto>> GetContactSubmissionsByEmailAsync(string email);
        Task<ContactSubmissionResponseDto> UpdateContactSubmissionAsync(Guid id, ContactSubmissionResponseDto dto);
        Task<bool> DeleteContactSubmissionAsync(Guid id);
        Task<Dictionary<string, int>> GetContactSubmissionStatsAsync();
    }
}

