using System.Text.Json;

namespace wx.Infrastructure.LLM;

public class SparkProvider : ILlmProvider
{
    private readonly HttpClient _httpClient;
    private readonly LlmConfig _config;

    public string Name => "Spark";

    public SparkProvider(LlmConfig config)
    {
        _httpClient = new HttpClient();
        _config = config;

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", $"{_config.ApiKey}:{_config.ApiSecret}");
    }

    public async Task<string> ChatCompletionsAsync(
        LlmMessage[] content,
        Dictionary<string, object>? parameters = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new Dictionary<string, object>()
            {
                { "messages", content },
            };

            if (parameters != null)
            {
                foreach (var kvp in parameters)
                {
                    request[kvp.Key] = kvp.Value;
                }
            }

            var body = new StringContent(JsonSerializer.Serialize(request));
            var response = await _httpClient.PostAsync($"{_config.BaseUrl}/chat/completions",
                                                       body,
                                                       cancellationToken);

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync(cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception($"Spark Error: {ex.Message}", ex);
        }
    }
}