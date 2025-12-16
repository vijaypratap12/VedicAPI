using VedicAPI.API.Models;
using VedicAPI.API.Models.DTOs;
using VedicAPI.API.Repositories.Interfaces;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly ILogger<ContactService> _logger;

        public ContactService(IContactRepository contactRepository, ILogger<ContactService> logger)
        {
            _contactRepository = contactRepository;
            _logger = logger;
        }

        public async Task<ContactSubmissionResponseDto> CreateContactSubmissionAsync(
            ContactSubmissionCreateDto dto, 
            string? ipAddress, 
            string? userAgent)
        {
            try
            {
                var contactSubmission = new ContactSubmission
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name,
                    Email = dto.Email,
                    Organization = dto.Organization,
                    Subject = dto.Subject,
                    Message = dto.Message,
                    ContactType = dto.ContactType,
                    Status = "Pending",
                    Priority = "Normal",
                    IpAddress = ipAddress,
                    UserAgent = userAgent,
                    SubmittedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var created = await _contactRepository.CreateAsync(contactSubmission);
                _logger.LogInformation("Contact submission created successfully for email: {Email}", dto.Email);

                // TODO: Send email notification to admin
                // TODO: Send confirmation email to user

                return MapToResponseDto(created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating contact submission");
                throw;
            }
        }

        public async Task<ContactSubmissionResponseDto?> GetContactSubmissionByIdAsync(Guid id)
        {
            try
            {
                var submission = await _contactRepository.GetByIdAsync(id);
                return submission != null ? MapToResponseDto(submission) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving contact submission with ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<ContactSubmissionResponseDto>> GetAllContactSubmissionsAsync()
        {
            try
            {
                var submissions = await _contactRepository.GetAllAsync();
                return submissions.Select(MapToResponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all contact submissions");
                throw;
            }
        }

        public async Task<IEnumerable<ContactSubmissionResponseDto>> GetContactSubmissionsByStatusAsync(string status)
        {
            try
            {
                var submissions = await _contactRepository.GetByStatusAsync(status);
                return submissions.Select(MapToResponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving contact submissions by status: {Status}", status);
                throw;
            }
        }

        public async Task<IEnumerable<ContactSubmissionResponseDto>> GetContactSubmissionsByTypeAsync(string contactType)
        {
            try
            {
                var submissions = await _contactRepository.GetByContactTypeAsync(contactType);
                return submissions.Select(MapToResponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving contact submissions by type: {ContactType}", contactType);
                throw;
            }
        }

        public async Task<IEnumerable<ContactSubmissionResponseDto>> GetContactSubmissionsByEmailAsync(string email)
        {
            try
            {
                var submissions = await _contactRepository.GetByEmailAsync(email);
                return submissions.Select(MapToResponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving contact submissions by email: {Email}", email);
                throw;
            }
        }

        public async Task<ContactSubmissionResponseDto> UpdateContactSubmissionAsync(Guid id, ContactSubmissionResponseDto dto)
        {
            try
            {
                var existing = await _contactRepository.GetByIdAsync(id);
                if (existing == null)
                {
                    throw new KeyNotFoundException($"Contact submission with ID {id} not found");
                }

                existing.Status = dto.Status;
                existing.Priority = dto.Priority;
                existing.RespondedAt = dto.RespondedAt;
                existing.RespondedBy = dto.RespondedBy;
                existing.ResponseMessage = dto.ResponseMessage;
                existing.IsArchived = dto.IsArchived;

                var updated = await _contactRepository.UpdateAsync(existing);
                _logger.LogInformation("Contact submission updated successfully: {Id}", id);

                return MapToResponseDto(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating contact submission with ID: {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteContactSubmissionAsync(Guid id)
        {
            try
            {
                var result = await _contactRepository.DeleteAsync(id);
                if (result)
                {
                    _logger.LogInformation("Contact submission deleted successfully: {Id}", id);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting contact submission with ID: {Id}", id);
                throw;
            }
        }

        public async Task<Dictionary<string, int>> GetContactSubmissionStatsAsync()
        {
            try
            {
                var stats = new Dictionary<string, int>
                {
                    { "Pending", await _contactRepository.GetCountByStatusAsync("Pending") },
                    { "InProgress", await _contactRepository.GetCountByStatusAsync("InProgress") },
                    { "Resolved", await _contactRepository.GetCountByStatusAsync("Resolved") },
                    { "Closed", await _contactRepository.GetCountByStatusAsync("Closed") }
                };

                return stats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving contact submission stats");
                throw;
            }
        }

        private static ContactSubmissionResponseDto MapToResponseDto(ContactSubmission submission)
        {
            return new ContactSubmissionResponseDto
            {
                Id = submission.Id,
                Name = submission.Name,
                Email = submission.Email,
                Organization = submission.Organization,
                Subject = submission.Subject,
                Message = submission.Message,
                ContactType = submission.ContactType,
                Status = submission.Status,
                Priority = submission.Priority,
                SubmittedAt = submission.SubmittedAt,
                RespondedAt = submission.RespondedAt,
                RespondedBy = submission.RespondedBy,
                ResponseMessage = submission.ResponseMessage,
                IsArchived = submission.IsArchived
            };
        }
    }
}

