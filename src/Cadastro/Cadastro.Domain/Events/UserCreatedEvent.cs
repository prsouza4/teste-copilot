namespace Cadastro.Domain.Events;

/// <summary>
/// Domain event raised when a new user is created.
/// </summary>
public sealed record UserCreatedEvent(
    Guid UserId,
    string Name,
    string Email) : DomainEvent;
