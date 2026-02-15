using VedicAPI.API.Models;
using VedicAPI.API.Models.DTOs;
using VedicAPI.API.Repositories.Interfaces;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Services
{
    /// <summary>
    /// Service implementation for Patient business logic
    /// </summary>
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly ILogger<PatientService> _logger;

        public PatientService(IPatientRepository patientRepository, ILogger<PatientService> logger)
        {
            _patientRepository = patientRepository;
            _logger = logger;
        }

        public async Task<PatientResponseDto?> GetPatientByIdAsync(long id)
        {
            try
            {
                var patient = await _patientRepository.GetByIdAsync(id);
                return patient != null ? MapToResponseDto(patient) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetPatientByIdAsync for ID {PatientId}", id);
                throw;
            }
        }

        public async Task<PatientResponseDto?> GetPatientByUserIdAsync(int userId)
        {
            try
            {
                var patient = await _patientRepository.GetByUserIdAsync(userId);
                return patient != null ? MapToResponseDto(patient) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetPatientByUserIdAsync for UserID {UserId}", userId);
                throw;
            }
        }

        public async Task<IEnumerable<PatientResponseDto>> GetAllPatientsAsync()
        {
            try
            {
                var patients = await _patientRepository.GetAllAsync();
                return patients.Select(MapToResponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllPatientsAsync");
                throw;
            }
        }

        public async Task<PatientResponseDto> CreatePatientAsync(PatientCreateDto patientDto)
        {
            try
            {
                var patient = MapToEntity(patientDto);
                var patientId = await _patientRepository.CreateAsync(patient);
                patient.Id = patientId;

                _logger.LogInformation("Patient created successfully with ID {PatientId}", patientId);
                return MapToResponseDto(patient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreatePatientAsync for patient {PatientName}", patientDto.Name);
                throw;
            }
        }

        public async Task<PatientResponseDto?> UpdatePatientAsync(long id, PatientUpdateDto patientDto)
        {
            try
            {
                var existingPatient = await _patientRepository.GetByIdAsync(id);
                if (existingPatient == null)
                {
                    _logger.LogWarning("Patient with ID {PatientId} not found for update", id);
                    return null;
                }

                var updatedPatient = MapToEntity(patientDto, id);
                updatedPatient.CreatedAt = existingPatient.CreatedAt;
                updatedPatient.Prakriti = existingPatient.Prakriti;
                updatedPatient.PrakritiScore = existingPatient.PrakritiScore;

                var success = await _patientRepository.UpdateAsync(updatedPatient);
                if (success)
                {
                    _logger.LogInformation("Patient with ID {PatientId} updated successfully", id);
                    return MapToResponseDto(updatedPatient);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdatePatientAsync for patient ID {PatientId}", id);
                throw;
            }
        }

        public async Task<bool> DeletePatientAsync(long id)
        {
            try
            {
                var exists = await _patientRepository.ExistsAsync(id);
                if (!exists)
                {
                    _logger.LogWarning("Patient with ID {PatientId} not found for deletion", id);
                    return false;
                }

                var success = await _patientRepository.DeleteAsync(id);
                if (success)
                {
                    _logger.LogInformation("Patient with ID {PatientId} deleted successfully", id);
                }

                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeletePatientAsync for patient ID {PatientId}", id);
                throw;
            }
        }

        #region Mapping Methods

        private static PatientResponseDto MapToResponseDto(Patient patient)
        {
            return new PatientResponseDto
            {
                Id = patient.Id,
                UserId = patient.UserId,
                Name = patient.Name,
                Age = patient.Age,
                Gender = patient.Gender,
                ContactNumber = patient.ContactNumber,
                Email = patient.Email,
                Address = patient.Address,
                Prakriti = patient.Prakriti,
                PrakritiScore = patient.PrakritiScore,
                CreatedAt = patient.CreatedAt,
                UpdatedAt = patient.UpdatedAt
            };
        }

        private static Patient MapToEntity(PatientCreateDto dto)
        {
            return new Patient
            {
                UserId = dto.UserId,
                Name = dto.Name,
                Age = dto.Age,
                Gender = dto.Gender,
                ContactNumber = dto.ContactNumber,
                Email = dto.Email,
                Address = dto.Address,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
        }

        private static Patient MapToEntity(PatientUpdateDto dto, long id)
        {
            return new Patient
            {
                Id = id,
                Name = dto.Name,
                Age = dto.Age,
                Gender = dto.Gender,
                ContactNumber = dto.ContactNumber,
                Email = dto.Email,
                Address = dto.Address,
                UpdatedAt = DateTime.UtcNow
            };
        }

        #endregion
    }
}
