using VedicAPI.API.Models.DTOs;
using VedicAPI.API.Repositories.Interfaces;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Services
{
    /// <summary>
    /// Service implementation for Treatment statistics business logic
    /// </summary>
    public class TreatmentStatisticsService : ITreatmentStatisticsService
    {
        private readonly ITreatmentRepository _treatmentRepository;
        private readonly ILogger<TreatmentStatisticsService> _logger;

        public TreatmentStatisticsService(
            ITreatmentRepository treatmentRepository,
            ILogger<TreatmentStatisticsService> logger)
        {
            _treatmentRepository = treatmentRepository;
            _logger = logger;
        }

        public async Task<TreatmentStatisticsDto> GetStatisticsAsync()
        {
            try
            {
                var stats = await _treatmentRepository.GetStatisticsAsync();

                var statisticsDto = new TreatmentStatisticsDto
                {
                    TotalPatients = GetIntValue(stats, "TotalPatients"),
                    TotalTreatmentPlans = GetIntValue(stats, "TotalTreatmentPlans"),
                    TotalConditions = GetIntValue(stats, "TotalConditions"),
                    TotalHerbalMedicines = GetIntValue(stats, "TotalHerbalMedicines"),
                    TotalYogaAsanas = GetIntValue(stats, "TotalYogaAsanas"),
                    AverageConfidenceScore = GetDecimalValue(stats, "AverageConfidenceScore"),
                    AverageEffectivenessScore = GetDecimalValue(stats, "AverageEffectivenessScore")
                };

                // Parse Prakriti distribution
                if (stats.ContainsKey("PrakritiDistribution"))
                {
                    var prakritiDist = stats["PrakritiDistribution"] as IEnumerable<dynamic>;
                    if (prakritiDist != null)
                    {
                        foreach (var item in prakritiDist)
                        {
                            statisticsDto.PrakritiDistribution[item.Prakriti] = (int)item.PatientCount;
                        }
                    }
                }

                // Parse top conditions
                if (stats.ContainsKey("TopConditions"))
                {
                    var topConditions = stats["TopConditions"] as IEnumerable<dynamic>;
                    if (topConditions != null)
                    {
                        statisticsDto.TopConditions = topConditions.Select(c => new ConditionStatDto
                        {
                            ConditionName = c.ConditionName,
                            TreatmentCount = (int)c.TreatmentCount
                        }).ToList();
                    }
                }

                // Parse top medicines
                if (stats.ContainsKey("TopMedicines"))
                {
                    var topMedicines = stats["TopMedicines"] as IEnumerable<dynamic>;
                    if (topMedicines != null)
                    {
                        statisticsDto.TopMedicines = topMedicines.Select(m => new MedicineStatDto
                        {
                            CommonName = m.CommonName,
                            SanskritName = m.SanskritName,
                            PrescriptionCount = (int)m.PrescriptionCount
                        }).ToList();
                    }
                }

                return statisticsDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetStatisticsAsync");
                throw;
            }
        }

        #region Helper Methods

        private int GetIntValue(Dictionary<string, object> stats, string key)
        {
            if (stats.ContainsKey(key) && stats[key] != null)
            {
                return Convert.ToInt32(stats[key]);
            }
            return 0;
        }

        private decimal GetDecimalValue(Dictionary<string, object> stats, string key)
        {
            if (stats.ContainsKey(key) && stats[key] != null)
            {
                return Convert.ToDecimal(stats[key]);
            }
            return 0m;
        }

        #endregion
    }
}
