namespace wx.Core.WorkFlow;

public interface IPipelineBuilder
{
    IPipelineBuilder Use(Func<PipelineDelegate,PipelineDelegate> middleware);

    PipelineDelegate Build();
}
