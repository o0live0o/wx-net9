namespace wx.Core.WorkFlow;

public class PipelineBuilder : IPipelineBuilder
{
    private readonly List<MiddlewareComponent> _components = new();

    public PipelineDelegate Build()
    {
        //default
        PipelineDelegate pipeline = context =>
        {
            Console.WriteLine("End!");
            return Task.CompletedTask;
        };

        foreach (var component in _components.AsEnumerable().Reverse())
        {
            pipeline = component.Compose(pipeline);
        }
        return pipeline;
    }

    public IPipelineBuilder Use(Func<PipelineDelegate, PipelineDelegate> middleware)
    {
        _components.Add(new MiddlewareComponent(middleware));
        return this;
    }
}
