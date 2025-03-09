using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wx.Core.WorkFlow;

public class PipelineExecutor
{
    public readonly PipelineDelegate _pipeline;
    public PipelineExecutor(IPipelineBuilder pipelineBuilder)
    {
        _pipeline = pipelineBuilder.Build();
    }

    public async Task ExcuteAsync(IPipelineContext context)
    {
        try
        {
            await _pipeline(context);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
