using Contract.Common.Config;
using Contract.Dtos.Responses.Ai;
using Microsoft.Extensions.Options;
using Service.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
namespace Service.Services
{
    public class AiService : IAiService
    {
        private readonly HttpClient _httpClient;
        private readonly AppSettingConfig.AiSettingConfig _aiSettingConfig;
        private readonly string _apiKey;
        private readonly string _apiUrl;
        private readonly string _model;
        private readonly string _userRole;
        private readonly string _systemRole;
        public AiService(IOptions<AppSettingConfig.AiSettingConfig> aiSettingConfig)
        {
            _httpClient = new HttpClient();
            _aiSettingConfig = aiSettingConfig.Value;
            _apiKey = _aiSettingConfig.ApiKey;
            _apiUrl = _aiSettingConfig.Url;
            _model = _aiSettingConfig.Model;
            _userRole = _aiSettingConfig.UserRole;
            _systemRole = _aiSettingConfig.SystemRole;
        }
        public async Task<string> SendChatAsync(string message)
        {
            string systemPrompt = @"
        You are a professional health assistant. Your guidelines:
        1. Only answer health-related questions 
        2. For non-health questions: 'This is outside my scope as a health assistant'
        3. For serious medical issues: 'Please consult a doctor immediately'
        4. Response should be concise short, clear, and to the point.
        5. Be empathetic but professional";
            var payload = new
            {
                model = _model,
                messages = new[]{
                new { role = _userRole, content = message},
                new { role = _systemRole, content = systemPrompt}
                }
            };
            var request = new HttpRequestMessage(HttpMethod.Post, _apiUrl)
            {
                Content = JsonContent.Create(payload)
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"API request failed: {response.StatusCode} - {errorContent}");
            }
            var responseJson = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Full API response: {responseJson}");
            var responseObj = JsonSerializer.Deserialize<AiResponse>(responseJson);
            string result = null;
            result = responseObj.choices.FirstOrDefault()?.message?.content;
            if (string.IsNullOrEmpty(result))
            {
                return result = "Server is busy";
            }
            return result;
        }

    }
}
