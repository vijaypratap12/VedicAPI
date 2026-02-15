using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using VedicAPI.API.Models;
using VedicAPI.API.Repositories.Interfaces;

namespace VedicAPI.API.Repositories
{
    /// <summary>
    /// Repository implementation for Treatment-related operations using Dapper
    /// </summary>
    public class TreatmentRepository : ITreatmentRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<TreatmentRepository> _logger;

        public TreatmentRepository(IConfiguration configuration, ILogger<TreatmentRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string cannot be null");
            _logger = logger;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        #region Treatment Plans

        public async Task<TreatmentPlan?> GetTreatmentPlanByIdAsync(long id)
        {
            try
            {
                using var connection = CreateConnection();
                var result = await connection.QueryAsync<TreatmentPlan>(
                    "sp_GetTreatmentPlanById",
                    new { TreatmentPlanId = id },
                    commandType: CommandType.StoredProcedure
                );
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving treatment plan with ID {PlanId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<TreatmentPlan>> GetTreatmentPlansByPatientIdAsync(long patientId)
        {
            try
            {
                using var connection = CreateConnection();
                return await connection.QueryAsync<TreatmentPlan>(
                    "sp_GetTreatmentHistory",
                    new { PatientId = patientId },
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving treatment plans for patient {PatientId}", patientId);
                throw;
            }
        }

        public async Task<long> CreateTreatmentPlanAsync(TreatmentPlan plan)
        {
            try
            {
                using var connection = CreateConnection();
                var result = await connection.QueryAsync<long>(
                    "sp_SaveTreatmentPlan",
                    new
                    {
                        plan.PatientId,
                        plan.ConditionId,
                        plan.Prakriti,
                        plan.HerbalMedicines,
                        plan.YogaAsanas,
                        plan.DietaryRecommendations,
                        plan.LifestyleModifications,
                        plan.Duration,
                        plan.ConfidenceScore,
                        plan.Explanation,
                        plan.CreatedBy
                    },
                    commandType: CommandType.StoredProcedure
                );
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating treatment plan for patient {PatientId}", plan.PatientId);
                throw;
            }
        }

        public async Task<bool> UpdateTreatmentPlanAsync(TreatmentPlan plan)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    UPDATE TREATMENTPLANS
                    SET HerbalMedicines = @HerbalMedicines,
                        YogaAsanas = @YogaAsanas,
                        DietaryRecommendations = @DietaryRecommendations,
                        LifestyleModifications = @LifestyleModifications,
                        Duration = @Duration,
                        ConfidenceScore = @ConfidenceScore,
                        Explanation = @Explanation,
                        UpdatedAt = GETUTCDATE()
                    WHERE Id = @Id";

                var rowsAffected = await connection.ExecuteAsync(sql, plan);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating treatment plan with ID {PlanId}", plan.Id);
                throw;
            }
        }

        #endregion

        #region Treatment Outcomes

        public async Task<long> SaveTreatmentOutcomeAsync(TreatmentOutcome outcome)
        {
            try
            {
                using var connection = CreateConnection();
                var result = await connection.QueryAsync<long>(
                    "sp_SaveTreatmentOutcome",
                    new
                    {
                        outcome.TreatmentPlanId,
                        outcome.PatientId,
                        outcome.EffectivenessScore,
                        outcome.SideEffects,
                        outcome.PatientFeedback,
                        outcome.DoctorNotes,
                        outcome.FollowUpDate,
                        outcome.RecordedBy
                    },
                    commandType: CommandType.StoredProcedure
                );
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving treatment outcome for plan {PlanId}", outcome.TreatmentPlanId);
                throw;
            }
        }

        public async Task<IEnumerable<TreatmentOutcome>> GetOutcomesByTreatmentPlanIdAsync(long treatmentPlanId)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    SELECT Id, TreatmentPlanId, PatientId, EffectivenessScore,
                           SideEffects, PatientFeedback, DoctorNotes, FollowUpDate,
                           RecordedAt, RecordedBy
                    FROM TREATMENTOUTCOMES
                    WHERE TreatmentPlanId = @TreatmentPlanId
                    ORDER BY RecordedAt DESC";

                return await connection.QueryAsync<TreatmentOutcome>(sql, new { TreatmentPlanId = treatmentPlanId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving outcomes for treatment plan {PlanId}", treatmentPlanId);
                throw;
            }
        }

        #endregion

        #region Treatment Recommendations

        public async Task<IEnumerable<HerbalMedicine>> GetRecommendedMedicinesAsync(long conditionId, string prakriti)
        {
            try
            {
                using var connection = CreateConnection();
                
                // Get the first result set (medicines)
                using var multi = await connection.QueryMultipleAsync(
                    "sp_GetTreatmentRecommendations",
                    new { ConditionId = conditionId, Prakriti = prakriti },
                    commandType: CommandType.StoredProcedure
                );

                return await multi.ReadAsync<HerbalMedicine>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving recommended medicines for condition {ConditionId}", conditionId);
                throw;
            }
        }

        public async Task<IEnumerable<YogaAsana>> GetRecommendedYogaAsanasAsync(long conditionId, string prakriti)
        {
            try
            {
                using var connection = CreateConnection();
                
                // Get the second result set (yoga asanas)
                using var multi = await connection.QueryMultipleAsync(
                    "sp_GetTreatmentRecommendations",
                    new { ConditionId = conditionId, Prakriti = prakriti },
                    commandType: CommandType.StoredProcedure
                );

                // Skip first result set (medicines)
                await multi.ReadAsync<HerbalMedicine>();
                // Read second result set (yoga)
                return await multi.ReadAsync<YogaAsana>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving recommended yoga asanas for condition {ConditionId}", conditionId);
                throw;
            }
        }

        public async Task<IEnumerable<DietaryItem>> GetDietaryItemsByPrakritiAsync(string prakriti, string? category = null)
        {
            try
            {
                using var connection = CreateConnection();
                return await connection.QueryAsync<DietaryItem>(
                    "sp_GetDietaryItemsByPrakriti",
                    new { Prakriti = prakriti, Category = category },
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving dietary items for Prakriti {Prakriti}", prakriti);
                throw;
            }
        }

        #endregion

        #region Statistics

        public async Task<Dictionary<string, object>> GetStatisticsAsync()
        {
            try
            {
                using var connection = CreateConnection();
                using var multi = await connection.QueryMultipleAsync(
                    "sp_GetTreatmentStatistics",
                    commandType: CommandType.StoredProcedure
                );

                var statistics = new Dictionary<string, object>();

                // First result set: Overall metrics
                var metrics = await multi.ReadAsync<dynamic>();
                foreach (var metric in metrics)
                {
                    statistics[metric.Metric] = metric.Value;
                }

                // Second result set: Prakriti distribution
                var prakritiDist = await multi.ReadAsync<dynamic>();
                statistics["PrakritiDistribution"] = prakritiDist;

                // Third result set: Top conditions
                var topConditions = await multi.ReadAsync<dynamic>();
                statistics["TopConditions"] = topConditions;

                // Fourth result set: Top medicines
                var topMedicines = await multi.ReadAsync<dynamic>();
                statistics["TopMedicines"] = topMedicines;

                return statistics;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving treatment statistics");
                throw;
            }
        }

        #endregion
    }
}
