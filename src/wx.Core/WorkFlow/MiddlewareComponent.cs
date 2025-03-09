namespace wx.Core.WorkFlow;

public class MiddlewareComponent
{
    private readonly Func<PipelineDelegate, PipelineDelegate> _middleware;

    public MiddlewareComponent(Func<PipelineDelegate,PipelineDelegate> middleware)
    {
        _middleware = middleware;
    }

    public PipelineDelegate Compose(PipelineDelegate next)
    {
        return _middleware(next);
    }
}
