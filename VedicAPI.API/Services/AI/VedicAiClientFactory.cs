using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Services.AI
{
    public class VedicAiClientFactory
    {
        private readonly IEnumerable<IVedicAiClient> _clients;
        private readonly ILogger<VedicAiClientFactory> _logger;

        public VedicAiClientFactory(IEnumerable<IVedicAiClient> clients, ILogger<VedicAiClientFactory> logger)
        {
            _clients = clients;
            _logger = logger;
        }

        public IVedicAiClient? GetClient(string providerName)
        {
            if (string.IsNullOrWhiteSpace(providerName))
                return _clients.FirstOrDefault(c => c.ProviderName.Equals("Gemini", StringComparison.OrdinalIgnoreCase));

            var client = _clients.FirstOrDefault(c => c.ProviderName.Equals(providerName, StringComparison.OrdinalIgnoreCase));
            if (client == null)
            {
                _logger.LogWarning("Configured AI provider {Provider} not found. Falling back to default provider.", providerName);
                return _clients.FirstOrDefault();
            }

            return client;
        }
    }
}
