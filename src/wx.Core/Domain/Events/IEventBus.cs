using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wx.Core.Domain.Events;

public interface IEventBus
{
    Task PublishAsync<T>(T integrationEvent,CancellationToken cancellationToken = default)
        where T : class, IIntegrationEvent;
}

public interface IIntegrationEvent : INotification
{
    Guid Id { get; init; }
}

public abstract record IntegrationEvent(Guid Id): IIntegrationEvent;
