namespace wx.Core;

public static partial class WxRetry
{
    public static void Run(Action func, int retry = 3, Action<Exception>? errorHandler = null)
    {
        try
        {
            func.Invoke();
        }
        catch (Exception ex)
        {
            errorHandler?.Invoke(ex);
            if (retry > 1)
                Run(func, --retry, errorHandler);
        }
    }
}