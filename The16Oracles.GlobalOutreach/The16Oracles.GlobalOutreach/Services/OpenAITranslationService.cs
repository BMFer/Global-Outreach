using Newtonsoft.Json;
using System.Text;
using The16Oracles.GlobalOutreach.Models;

namespace The16Oracles.GlobalOutreach.Services
{
    public class OpenAITranslationService : ITranslationService
    {
        private readonly HttpClient _httpClient;
        private readonly BotConfiguration _config;
        private const string OpenAIApiUrl = "https://api.openai.com/v1/chat/completions";

        public OpenAITranslationService(HttpClient httpClient, BotConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.OpenAIApiKey}");
        }

        public async Task<string> DetectLanguageAsync(string text)
        {
            var systemPrompt = "You are a language detection assistant. Respond with ONLY the language name in English (e.g., 'Spanish', 'French', 'Japanese'). Do not include any other text.";
            var userPrompt = $"Detect the language of this text: {text}";

            var response = await CallOpenAIAsync(systemPrompt, userPrompt);
            return response.Trim();
        }

        public async Task<string> TranslateAsync(string text, string targetLanguage, string sourceLanguage = "auto")
        {
            string systemPrompt;

            if (sourceLanguage == "auto" || string.IsNullOrEmpty(sourceLanguage))
            {
                systemPrompt = $"You are a professional translator. Translate the following text to {targetLanguage}. Preserve the tone and meaning. Respond with ONLY the translation, no explanations.";
            }
            else
            {
                systemPrompt = $"You are a professional translator. Translate the following text from {sourceLanguage} to {targetLanguage}. Preserve the tone and meaning. Respond with ONLY the translation, no explanations.";
            }

            var response = await CallOpenAIAsync(systemPrompt, text);
            return response.Trim();
        }

        private async Task<string> CallOpenAIAsync(string systemPrompt, string userPrompt)
        {
            var requestBody = new
            {
                model = _config.OpenAIModel,
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = userPrompt }
                },
                temperature = 0.3,
                max_tokens = 1000
            };

            var jsonContent = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(OpenAIApiUrl, content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                dynamic? result = JsonConvert.DeserializeObject(responseBody);

                if (result?.choices != null && result!.choices.Count > 0)
                {
                    string contentValue = result!.choices[0].message.content?.ToString() ?? string.Empty;
                    return contentValue;
                }

                throw new Exception("No response from OpenAI");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error calling OpenAI API: {ex.Message}", ex);
            }
        }
    }
}
