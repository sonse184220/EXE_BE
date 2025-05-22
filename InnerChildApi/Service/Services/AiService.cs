using Contract.Common.Config;
using Contract.Dtos.Responses.Ai;
using Contract.Dtos.Responses.Chat;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
namespace Service.Services
{
    public interface IAiService
    {
        Task<string> SendChatAsync(string message, List<AllChatHistoryResponse> chatHistory);
    }
    public class AiService : IAiService
    {
        private readonly HttpClient _httpClient;
        private readonly AppSettingConfig.AiSettingConfig _aiSettingConfig;
        private readonly string _apiKey;
        private readonly string _apiUrl;
        private readonly string _model;
        private readonly string _userRole;
        private readonly string _systemRole;
        private readonly string _assistantRole;
        public AiService(IOptions<AppSettingConfig.AiSettingConfig> aiSettingConfig)
        {
            _httpClient = new HttpClient();
            _aiSettingConfig = aiSettingConfig.Value;
            _apiKey = _aiSettingConfig.ApiKey;
            _apiUrl = _aiSettingConfig.Url;
            _model = _aiSettingConfig.Model;
            _userRole = _aiSettingConfig.UserRole;
            _systemRole = _aiSettingConfig.SystemRole;
            _assistantRole = _aiSettingConfig.AssistantRole;
        }
        public async Task<string> SendChatAsync(string message, List<AllChatHistoryResponse> chatHistory)
        {
            string systemPrompt = @"
You are a friendly health assistant. Follow these rules:
1. Only answer health questions.
2. If not health-related, say: ""This is outside my scope.""
3. For serious health issues, say: ""Please see a doctor immediately.""
4. Keep answers short, clear, simple in English or Vietnamese.
5. Be polite and professional.
";
            var messageList = new List<object>();
            messageList.Add(new { role = _systemRole, content = systemPrompt });
            foreach (var item in chatHistory)
            {
                string role = item.Role.ToLower() switch
                {
                    "user" => "user",
                    "assistant" => "assistant",
                    "system" => "assistant",
                    _ => "user",
                };
                messageList.Add(new { role = role, content = item.Content });
            }
            messageList.Add(new { role = _userRole, content = message });
            var payload = new
            {
                model = _model,
                messages = messageList,
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
