namespace wx.Core.WorkFlow;

public interface IPipelineContext
{
    string Name { get; }
}

public class BasePipelineContext : IPipelineContext
{
    public string Name => "Base";
}

public delegate Task PipelineDelegate(IPipelineContext context);