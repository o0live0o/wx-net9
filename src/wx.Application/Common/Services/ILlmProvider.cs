using wx.Application.Common.Services.Dtos;

namespace wx.Application.Services;

public interface ILlmProvider
{
    string Name { get; }
    Task<string> ChatCompletionsAsync(LlmMessage[] content,
                                      Dictionary<string, object>? parameters = null,
                                      CancellationToken cancellationToken = default);
}
