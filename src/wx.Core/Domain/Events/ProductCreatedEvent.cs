namespace wx.Core.Domain.Events;

public record ProductCreatedEvent(Guid id, Product product) : IntegrationEvent(id);