using Microsoft.Extensions.Hosting;
using wx.Core.Domain.Events;

namespace wx.Application.Events
{
    public class IntergrationEventProcessorJob(InMemoryMessageQueue queue,IPublisher publisher) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach(var @event in queue.Reader.ReadAllAsync(stoppingToken))
            {
                await publisher.Publish(@event,stoppingToken);
            }
        }
    }
}
