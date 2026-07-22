using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using VedicAPI.API.Models;
using VedicAPI.API.Models.DTOs;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Services.AI
{
    public class GeminiAiClient : IVedicAiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GeminiAiClient> _logger;

        public string ProviderName => "Gemini";

        public GeminiAiClient(HttpClient httpClient, ILogger<GeminiAiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<TreatmentRecommendationDto?> GenerateClinicalRecommendationAsync(
            Patient patient,
            Condition condition,
            IEnumerable<HerbalMedicine> candidateMedicines,
            IEnumerable<YogaAsana> candidateYoga,
            IEnumerable<DietaryItem> candidateDietary,
            string apiKey,
            string modelName,
            string? customClinicalNotes = null)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                _logger.LogWarning("Gemini API key is empty. Falling back to DB rule engine.");
                return null;
            }

            try
            {
                var model = string.IsNullOrWhiteSpace(modelName) ? "gemini-1.5-flash" : modelName;
                var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

                var systemAndUserPrompt = BuildAyurvedicPrompt(patient, condition, candidateMedicines, candidateYoga, candidateDietary, customClinicalNotes);

                var payload = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = systemAndUserPrompt }
                            }
                        }
                    },
                    generationConfig = new
                    {
                        responseMimeType = "application/json",
                        temperature = 0.2
                    }
                };

                var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(endpoint, jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    var errBody = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Gemini API call failed with HTTP status {Status}: {Error}", response.StatusCode, errBody);
                    return null;
                }

                var responseString = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(responseString);
                
                var root = doc.RootElement;
                if (!root.TryGetProperty("candidates", out var candidates) || candidates.GetArrayLength() == 0)
                {
                    _logger.LogWarning("No candidates returned from Gemini API.");
                    return null;
                }

                var rawJsonText = candidates[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                if (string.IsNullOrWhiteSpace(rawJsonText)) return null;

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var aiData = JsonSerializer.Deserialize<AiResponseStructure>(rawJsonText, options);

                if (aiData == null) return null;

                // Map parsed AI output into TreatmentRecommendationDto
                return new TreatmentRecommendationDto
                {
                    PatientId = patient.Id,
                    PatientName = patient.Name,
                    Prakriti = patient.Prakriti ?? "Vata-Pitta",
                    ConditionId = condition.Id,
                    ConditionName = condition.Name,
                    ConfidenceScore = aiData.ConfidenceScore > 0 ? (decimal)aiData.ConfidenceScore : 92.5m,
                    Explanation = aiData.Explanation ?? $"AI Clinical assessment for {condition.Name}",
                    ClinicalRationale = aiData.ClinicalRationale ?? string.Empty,
                    IsAiGenerated = true,
                    AiModelUsed = $"{ProviderName} ({model})",
                    RecommendedMedicines = MapMedicines(candidateMedicines, aiData.Medicines),
                    RecommendedYogaAsanas = MapYoga(candidateYoga, aiData.YogaAsanas),
                    RecommendedDietaryItems = MapDietary(candidateDietary, aiData.PathyaList),
                    LifestyleModifications = aiData.LifestyleModifications ?? new List<string>(),
                    AnupanaInstructions = aiData.AnupanaInstructions ?? new Dictionary<string, string>(),
                    PathyaList = aiData.PathyaList ?? new List<string>(),
                    ApathyaList = aiData.ApathyaList ?? new List<string>(),
                    Warnings = aiData.Warnings ?? new List<string>()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception encountered during Gemini API recommendation generation.");
                return null;
            }
        }

        public async Task<TreatmentRecommendationDto?> SuggestAdjustmentAsync(
            Patient patient,
            Condition condition,
            TreatmentPlanResponseDto currentPlan,
            IEnumerable<TreatmentOutcomeDto> outcomes,
            string apiKey,
            string modelName)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                _logger.LogWarning("Gemini API key is empty. Cannot suggest adjustment.");
                return null;
            }

            try
            {
                var model = string.IsNullOrWhiteSpace(modelName) ? "gemini-1.5-flash" : modelName;
                var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

                var systemAndUserPrompt = BuildAdjustmentPrompt(patient, condition, currentPlan, outcomes);

                var payload = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = systemAndUserPrompt }
                            }
                        }
                    },
                    generationConfig = new
                    {
                        responseMimeType = "application/json",
                        temperature = 0.2
                    }
                };

                var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(endpoint, jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    var errBody = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Gemini API call failed with HTTP status {Status}: {Error}", response.StatusCode, errBody);
                    return null;
                }

                var responseString = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(responseString);
                
                var root = doc.RootElement;
                if (!root.TryGetProperty("candidates", out var candidates) || candidates.GetArrayLength() == 0)
                {
                    _logger.LogWarning("No candidates returned from Gemini API.");
                    return null;
                }

                var rawJsonText = candidates[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                if (string.IsNullOrWhiteSpace(rawJsonText)) return null;

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var aiData = JsonSerializer.Deserialize<AiResponseStructure>(rawJsonText, options);

                if (aiData == null) return null;

                return new TreatmentRecommendationDto
                {
                    PatientId = patient.Id,
                    PatientName = patient.Name,
                    Prakriti = patient.Prakriti ?? "Vata-Pitta",
                    ConditionId = condition.Id,
                    ConditionName = condition.Name,
                    ConfidenceScore = aiData.ConfidenceScore > 0 ? (decimal)aiData.ConfidenceScore : 92.5m,
                    Explanation = aiData.Explanation ?? $"AI Adjustment suggestion for {condition.Name}",
                    ClinicalRationale = aiData.ClinicalRationale ?? string.Empty,
                    IsAiGenerated = true,
                    AiModelUsed = $"{ProviderName} ({model})",
                    RecommendedMedicines = aiData.Medicines?.Select(m => new HerbalMedicineDto
                    {
                        CommonName = m.CommonName ?? "Ayurvedic Formulation",
                        SanskritName = m.SanskritName,
                        Dosage = m.Dosage ?? "As directed by physician"
                    }).ToList() ?? new List<HerbalMedicineDto>(),
                    RecommendedYogaAsanas = aiData.YogaAsanas?.Select(y => new YogaAsanaDto
                    {
                        AsanaName = y.AsanaName ?? "Yogic Posture",
                        SanskritName = y.SanskritName,
                        Duration = y.Duration ?? "5-10 minutes"
                    }).ToList() ?? new List<YogaAsanaDto>(),
                    RecommendedDietaryItems = aiData.PathyaList?.Select(p => new DietaryItemDto
                    {
                        FoodName = p,
                        Category = "Pathya (Recommended Diet)",
                        Benefits = "Favored for Dosha balance"
                    }).ToList() ?? new List<DietaryItemDto>(),
                    LifestyleModifications = aiData.LifestyleModifications ?? new List<string>(),
                    AnupanaInstructions = aiData.AnupanaInstructions ?? new Dictionary<string, string>(),
                    PathyaList = aiData.PathyaList ?? new List<string>(),
                    ApathyaList = aiData.ApathyaList ?? new List<string>(),
                    Warnings = aiData.Warnings ?? new List<string>()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception encountered during Gemini API suggestion generation.");
                return null;
            }
        }

        private string BuildAdjustmentPrompt(
            Patient patient,
            Condition condition,
            TreatmentPlanResponseDto currentPlan,
            IEnumerable<TreatmentOutcomeDto> outcomes)
        {
            var outcomesJson = JsonSerializer.Serialize(outcomes.Select(o => new { o.EffectivenessScore, o.SideEffects, o.PatientFeedback, o.DoctorNotes, o.FollowUpDate }));

            var sb = new StringBuilder();
            sb.AppendLine("You are a Senior Ayurvedic Practitioner and Shalya Tantra Specialist at Vedic Surgery Portal.");
            sb.AppendLine("Review the patient profile, diagnosed condition, current treatment plan, and logged clinical outcomes. Then suggest modifications/adjustments to the treatment plan in JSON format.");
            sb.AppendLine();
            sb.AppendLine("PATIENT PROFILE:");
            sb.AppendLine($"- Name: {patient.Name}, Age: {patient.Age}, Gender: {patient.Gender}, Prakriti: {patient.Prakriti}");
            sb.AppendLine();
            sb.AppendLine("DIAGNOSED CONDITION:");
            sb.AppendLine($"- Name: {condition.Name} ({condition.SanskritName})");
            sb.AppendLine();
            sb.AppendLine("CURRENT ACTIVE TREATMENT PLAN:");
            sb.AppendLine($"- Herbal Medicines: {currentPlan.HerbalMedicines}");
            sb.AppendLine($"- Yoga Asanas: {currentPlan.YogaAsanas}");
            sb.AppendLine($"- Dietary Recommendations: {currentPlan.DietaryRecommendations}");
            sb.AppendLine($"- Lifestyle Modifications: {currentPlan.LifestyleModifications}");
            sb.AppendLine($"- Duration: {currentPlan.Duration}");
            sb.AppendLine($"- Explanation: {currentPlan.Explanation}");
            sb.AppendLine();
            sb.AppendLine("LOGGED CLINICAL OUTCOMES / VISIT HISTORY:");
            sb.AppendLine(outcomesJson);
            sb.AppendLine();
            sb.AppendLine("Based on patient outcomes, adjust or refine the plan. If effectiveness is low or side effects occur, change the herbal medicines, dosages, yoga, or diet accordingly. If patient is improving, maintain or taper the plan.");
            sb.AppendLine();
            sb.AppendLine("REQUIRED JSON OUTPUT FORMAT (Strict JSON only, matching key names exactly, no markdown wrappers):");
            sb.AppendLine("{");
            sb.AppendLine("  \"confidenceScore\": 95.0,");
            sb.AppendLine("  \"explanation\": \"Summarize the adjustment rationale (e.g. 'Increasing Vata pacification due to persistent joint stiffness').\",");
            sb.AppendLine("  \"clinicalRationale\": \"Ayurvedic clinical analysis of why this adjustment is made based on outcomes.\",");
            sb.AppendLine("  \"medicines\": [");
            sb.AppendLine("    {");
            sb.AppendLine("      \"commonName\": \"Ashwagandha Churna\",");
            sb.AppendLine("      \"sanskritName\": \"अश्वगन्धा\",");
            sb.AppendLine("      \"dosage\": \"5g twice daily (increased from 3g)\",");
            sb.AppendLine("      \"anupana\": \"Warm milk\"");
            sb.AppendLine("    }");
            sb.AppendLine("  ],");
            sb.AppendLine("  \"yogaAsanas\": [");
            sb.AppendLine("    {");
            sb.AppendLine("      \"asanaName\": \"Trikonasana\",");
            sb.AppendLine("      \"sanskritName\": \"त्रिकोणासन\",");
            sb.AppendLine("      \"duration\": \"10 minutes daily\"");
            sb.AppendLine("    }");
            sb.AppendLine("  ],");
            sb.AppendLine("  \"anupanaInstructions\": {");
            sb.AppendLine("    \"Ashwagandha Churna\": \"Take with warm milk after food.\"");
            sb.AppendLine("  },");
            sb.AppendLine("  \"pathyaList\": [\"Warm cooked grains\", \"Ghee\"],");
            sb.AppendLine("  \"apathyaList\": [\"Cold drinks\"],");
            sb.AppendLine("  \"lifestyleModifications\": [\"Gentle walk daily\"],");
            sb.AppendLine("  \"warnings\": [\"Monitor digestion.\"]");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private string BuildAyurvedicPrompt(
            Patient patient,
            Condition condition,
            IEnumerable<HerbalMedicine> candidateMedicines,
            IEnumerable<YogaAsana> candidateYoga,
            IEnumerable<DietaryItem> candidateDietary,
            string? customNotes)
        {
            var medsJson = JsonSerializer.Serialize(candidateMedicines.Select(m => new { m.Id, m.CommonName, m.SanskritName, m.Properties, m.Indications }));
            var yogaJson = JsonSerializer.Serialize(candidateYoga.Select(y => new { y.Id, y.AsanaName, y.SanskritName, y.Benefits, y.Duration }));
            var dietJson = JsonSerializer.Serialize(candidateDietary.Take(10).Select(d => new { d.FoodName, d.Category, d.VataEffect, d.PittaEffect, d.KaphaEffect }));

            var sb = new StringBuilder();
            sb.AppendLine("You are a Senior Ayurvedic Practitioner and Shalya Tantra (Surgical & Clinical Ayurveda) Specialist at Vedic Surgery Portal.");
            sb.AppendLine("Analyze the patient profile and condition, then generate a personalized, evidence-based Ayurvedic treatment recommendation in JSON format.");
            sb.AppendLine();
            sb.AppendLine("PATIENT PROFILE:");
            sb.AppendLine($"- Name: {patient.Name}");
            sb.AppendLine($"- Age: {patient.Age}, Gender: {patient.Gender}");
            sb.AppendLine($"- Prakriti (Constitution): {patient.Prakriti ?? "Vata-Pitta"}");
            sb.AppendLine($"- Additional Notes: {customNotes ?? "None provided"}");
            sb.AppendLine();
            sb.AppendLine("DIAGNOSED CONDITION:");
            sb.AppendLine($"- Name: {condition.Name} ({condition.SanskritName ?? "Vyadhi"})");
            sb.AppendLine($"- Category: {condition.Category}");
            sb.AppendLine($"- Affected Doshas: {condition.AffectedDoshas}");
            sb.AppendLine($"- Description: {condition.Description}");
            sb.AppendLine();
            sb.AppendLine("AVAILABLE DATABASE CANDIDATES (Choose and refine the best items from here):");
            sb.AppendLine($"- Medicines: {medsJson}");
            sb.AppendLine($"- Yoga Poses: {yogaJson}");
            sb.AppendLine($"- Dietary Items: {dietJson}");
            sb.AppendLine();
            sb.AppendLine("REQUIRED JSON OUTPUT FORMAT (Strict JSON only, no markdown wrappers):");
            sb.AppendLine("{");
            sb.AppendLine("  \"confidenceScore\": 95.0,");
            sb.AppendLine("  \"explanation\": \"Brief 2-sentence summary of the treatment strategy.\",");
            sb.AppendLine($"  \"clinicalRationale\": \"Detailed Ayurvedic pathogenesis (Samprapti) breakdown explaining why these specific herbs and practices pacify the patient's aggravated Doshas and treat {condition.Name}.\",");
            sb.AppendLine("  \"medicines\": [");
            sb.AppendLine("    {");
            sb.AppendLine("      \"commonName\": \"Ashwagandha Churna\",");
            sb.AppendLine("      \"sanskritName\": \"अश्वगन्धा\",");
            sb.AppendLine("      \"dosage\": \"3g twice daily after meals\",");
            sb.AppendLine("      \"anupana\": \"Warm milk with a pinch of nutmeg\"");
            sb.AppendLine("    }");
            sb.AppendLine("  ],");
            sb.AppendLine("  \"yogaAsanas\": [");
            sb.AppendLine("    {");
            sb.AppendLine("      \"asanaName\": \"Trikonasana\",");
            sb.AppendLine("      \"sanskritName\": \"त्रिकोणासन\",");
            sb.AppendLine("      \"duration\": \"5-7 minutes daily\"");
            sb.AppendLine("    }");
            sb.AppendLine("  ],");
            sb.AppendLine("  \"anupanaInstructions\": {");
            sb.AppendLine("    \"Ashwagandha Churna\": \"Take with warm milk at bedtime for Vata pacification and Rasayana benefit.\"");
            sb.AppendLine("  },");
            sb.AppendLine("  \"pathyaList\": [\"Warm cooked grains\", \"Cow's Ghee\", \"Sesame seeds\", \"Moong dal soup\"],");
            sb.AppendLine("  \"apathyaList\": [\"Cold icy drinks\", \"Raw salads\", \"Excessive dry spicy foods\", \"Nighttime curds\"],");
            sb.AppendLine("  \"lifestyleModifications\": [");
            sb.AppendLine("    \"Maintain regular sleep schedule (10 PM to 6 AM)\",");
            sb.AppendLine("    \"Perform gentle warm oil Abhyanga massage daily\"");
            sb.AppendLine("  ],");
            sb.AppendLine("  \"warnings\": [");
            sb.AppendLine("    \"Monitor digestive fire (Agni); reduce dosage if heaviness occurs.\"");
            sb.AppendLine("  ]");
            sb.AppendLine("}");

            return sb.ToString();
        }

        #region Helper Mapping

        private List<HerbalMedicineDto> MapMedicines(IEnumerable<HerbalMedicine> candidateMedicines, List<AiMedicineItem>? aiMeds)
        {
            if (aiMeds == null || !aiMeds.Any())
            {
                return candidateMedicines.Take(3).Select(m => new HerbalMedicineDto
                {
                    Id = m.Id,
                    CommonName = m.CommonName,
                    SanskritName = m.SanskritName,
                    Dosage = m.Dosage ?? "As directed by physician"
                }).ToList();
            }

            var result = new List<HerbalMedicineDto>();
            foreach (var aiMed in aiMeds)
            {
                var match = candidateMedicines.FirstOrDefault(m => 
                    m.CommonName.Contains(aiMed.CommonName ?? "", StringComparison.OrdinalIgnoreCase) || 
                    (aiMed.SanskritName != null && m.SanskritName != null && m.SanskritName.Contains(aiMed.SanskritName, StringComparison.OrdinalIgnoreCase)));

                result.Add(new HerbalMedicineDto
                {
                    Id = match?.Id ?? 0,
                    CommonName = aiMed.CommonName ?? match?.CommonName ?? "Ayurvedic Formulation",
                    SanskritName = aiMed.SanskritName ?? match?.SanskritName,
                    Dosage = aiMed.Dosage ?? match?.Dosage ?? "3g twice daily"
                });
            }
            return result;
        }

        private List<YogaAsanaDto> MapYoga(IEnumerable<YogaAsana> candidateYoga, List<AiYogaItem>? aiYoga)
        {
            if (aiYoga == null || !aiYoga.Any())
            {
                return candidateYoga.Take(3).Select(y => new YogaAsanaDto
                {
                    Id = y.Id,
                    AsanaName = y.AsanaName,
                    SanskritName = y.SanskritName,
                    Duration = y.Duration ?? "5-10 minutes"
                }).ToList();
            }

            var result = new List<YogaAsanaDto>();
            foreach (var a in aiYoga)
            {
                var match = candidateYoga.FirstOrDefault(y => 
                    y.AsanaName.Contains(a.AsanaName ?? "", StringComparison.OrdinalIgnoreCase));

                result.Add(new YogaAsanaDto
                {
                    Id = match?.Id ?? 0,
                    AsanaName = a.AsanaName ?? match?.AsanaName ?? "Yogic Posture",
                    SanskritName = a.SanskritName ?? match?.SanskritName,
                    Duration = a.Duration ?? match?.Duration ?? "5-10 minutes"
                });
            }
            return result;
        }

        private List<DietaryItemDto> MapDietary(IEnumerable<DietaryItem> candidateDietary, List<string>? pathya)
        {
            if (pathya == null || !pathya.Any())
            {
                return candidateDietary.Take(4).Select(d => new DietaryItemDto
                {
                    Id = d.Id,
                    FoodName = d.FoodName,
                    Category = d.Category,
                    Benefits = d.Benefits
                }).ToList();
            }

            return pathya.Select(p => new DietaryItemDto
            {
                FoodName = p,
                Category = "Pathya (Recommended Diet)",
                Benefits = "Favored for Dosha balance"
            }).ToList();
        }

        #endregion

        #region Inner DTO Classes for AI Parsing

        private class AiResponseStructure
        {
            [JsonPropertyName("confidenceScore")]
            public double ConfidenceScore { get; set; }

            [JsonPropertyName("explanation")]
            public string? Explanation { get; set; }

            [JsonPropertyName("clinicalRationale")]
            public string? ClinicalRationale { get; set; }

            [JsonPropertyName("medicines")]
            public List<AiMedicineItem>? Medicines { get; set; }

            [JsonPropertyName("yogaAsanas")]
            public List<AiYogaItem>? YogaAsanas { get; set; }

            [JsonPropertyName("anupanaInstructions")]
            public Dictionary<string, string>? AnupanaInstructions { get; set; }

            [JsonPropertyName("pathyaList")]
            public List<string>? PathyaList { get; set; }

            [JsonPropertyName("apathyaList")]
            public List<string>? ApathyaList { get; set; }

            [JsonPropertyName("lifestyleModifications")]
            public List<string>? LifestyleModifications { get; set; }

            [JsonPropertyName("warnings")]
            public List<string>? Warnings { get; set; }
        }

        private class AiMedicineItem
        {
            [JsonPropertyName("commonName")]
            public string? CommonName { get; set; }

            [JsonPropertyName("sanskritName")]
            public string? SanskritName { get; set; }

            [JsonPropertyName("dosage")]
            public string? Dosage { get; set; }

            [JsonPropertyName("anupana")]
            public string? Anupana { get; set; }
        }

        private class AiYogaItem
        {
            [JsonPropertyName("asanaName")]
            public string? AsanaName { get; set; }

            [JsonPropertyName("sanskritName")]
            public string? SanskritName { get; set; }

            [JsonPropertyName("duration")]
            public string? Duration { get; set; }
        }

        #endregion
    }
}
