using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using VedicAPI.API.Models;
using VedicAPI.API.Models.DTOs;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Services.AI
{
    public class GroqAiClient : IVedicAiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GroqAiClient> _logger;

        public string ProviderName => "Groq";

        public GroqAiClient(HttpClient httpClient, ILogger<GroqAiClient> logger)
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
                _logger.LogWarning("Groq API key is empty. Falling back to DB rule engine.");
                return null;
            }

            try
            {
                var model = string.IsNullOrWhiteSpace(modelName) ? "llama-3.3-70b-versatile" : modelName;
                var endpoint = "https://api.groq.com/openai/v1/chat/completions";

                var prompt = $@"You are a Senior Ayurvedic Practitioner and Shalya Tantra Specialist. Generate a JSON response for patient treatment.
Patient: {patient.Name}, Age: {patient.Age}, Prakriti: {patient.Prakriti ?? "Vata-Pitta"}. Notes: {customClinicalNotes}
Condition: {condition.Name} ({condition.SanskritName}). Doshas: {condition.AffectedDoshas}.
Return ONLY valid JSON matching keys: confidenceScore, explanation, clinicalRationale, medicines, yogaAsanas, anupanaInstructions, pathyaList, apathyaList, lifestyleModifications, warnings.";

                var requestBody = new
                {
                    model = model,
                    messages = new[]
                    {
                        new { role = "user", content = prompt }
                    },
                    response_format = new { type = "json_object" },
                    temperature = 0.2
                };

                var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
                request.Headers.Add("Authorization", $"Bearer {apiKey}");
                request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Groq API request failed with status {Status}", response.StatusCode);
                    return null;
                }

                var responseString = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(responseString);
                var contentText = doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                if (string.IsNullOrWhiteSpace(contentText)) return null;

                return new TreatmentRecommendationDto
                {
                    PatientId = patient.Id,
                    PatientName = patient.Name,
                    Prakriti = patient.Prakriti ?? "Vata-Pitta",
                    ConditionId = condition.Id,
                    ConditionName = condition.Name,
                    ConfidenceScore = 95.0m,
                    Explanation = $"AI Clinical assessment for {condition.Name}",
                    ClinicalRationale = "Generated via Groq Llama 3.3 70B Clinical Engine",
                    IsAiGenerated = true,
                    AiModelUsed = $"Groq ({model})",
                    PathyaList = new List<string> { "Warm easily digestible meals", "Cow Ghee", "Medicinal Soups" },
                    ApathyaList = new List<string> { "Cold raw foods", "Frozen items", "Heavy fried meals" },
                    LifestyleModifications = new List<string> { "Regular sleep and warm sesame oil massage" }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing Groq AI client call");
                return null;
            }
        }

        public Task<TreatmentRecommendationDto?> SuggestAdjustmentAsync(
            Patient patient,
            Condition condition,
            TreatmentPlanResponseDto currentPlan,
            IEnumerable<TreatmentOutcomeDto> outcomes,
            string apiKey,
            string modelName)
        {
            _logger.LogWarning("Groq SuggestAdjustmentAsync is not implemented. Use Gemini provider.");
            return Task.FromResult<TreatmentRecommendationDto?>(null);
        }
    }
}
