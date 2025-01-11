using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace wx.Application.Events;

public class EventBus(InMemoryMessageQueue queue) : IEventBus
{
    async Task IEventBus.PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken)
    {
        await queue.Writer.WriteAsync(integrationEvent, cancellationToken);
    }
}

public class InMemoryMessageQueue
{
    private readonly Channel<IIntegrationEvent> _channel = Channel.CreateUnbounded<IIntegrationEvent>();

    public ChannelWriter<IIntegrationEvent> Writer => _channel.Writer;

    public ChannelReader<IIntegrationEvent> Reader => _channel.Reader;
}
