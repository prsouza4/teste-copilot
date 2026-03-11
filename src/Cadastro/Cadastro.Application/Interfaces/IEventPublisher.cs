using Cadastro.Domain.Events;

namespace Cadastro.Application.Interfaces;

/// <summary>
/// Interface for publishing domain events to a message broker.
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// Publishes a domain event asynchronously.
    /// </summary>
    /// <param name="domainEvent">The event to publish.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task PublishAsync(DomainEvent domainEvent, CancellationToken cancellationToken = default);
}
