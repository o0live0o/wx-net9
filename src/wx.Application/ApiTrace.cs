using Microsoft.Extensions.Logging;

namespace wx.Application;

public static partial class ApiTrace
{
    [LoggerMessage(EventId = 1, EventName = nameof(LogRequest), Level = LogLevel.Information, Message = "Request id： {requestId}，Msg: {msg}")]
    public static partial void LogRequest(ILogger logger, int requestId, string msg);
}
