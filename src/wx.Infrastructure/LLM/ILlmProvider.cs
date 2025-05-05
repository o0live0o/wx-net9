namespace wx.Infrastructure.LLM;

public interface ILlmProvider
{
    string Name { get; }
    Task<string> ChatCompletionsAsync(LlmMessage[] content,
                                      Dictionary<string, object>? parameters = null,
                                      CancellationToken cancellationToken = default);
}
