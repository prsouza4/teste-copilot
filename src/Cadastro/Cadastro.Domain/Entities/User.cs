using Cadastro.Domain.Events;
using Cadastro.Domain.Exceptions;
using Cadastro.Domain.ValueObjects;

namespace Cadastro.Domain.Entities;

/// <summary>
/// User aggregate root.
/// </summary>
public sealed class User
{
    private readonly List<DomainEvent> _domainEvents = [];

    public Guid Id { get; private set; }
    public Name Name { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public bool IsActive { get; private set; }

    /// <summary>
    /// Gets the domain events raised by this aggregate.
    /// </summary>
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    // Private constructor for EF Core
    private User()
    {
    }

    /// <summary>
    /// Factory method to create a new User.
    /// </summary>
    /// <param name="name">User's name.</param>
    /// <param name="email">User's email.</param>
    /// <returns>A new User instance with a UserCreatedEvent raised.</returns>
    public static User Create(string name, string email)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = ValueObjects.Name.Create(name),
            Email = ValueObjects.Email.Create(email),
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        user.RaiseDomainEvent(new UserCreatedEvent(
            user.Id,
            user.Name.Value,
            user.Email.Value));

        return user;
    }

    /// <summary>
    /// Deactivates the user.
    /// </summary>
    public void Deactivate()
    {
        if (!IsActive)
        {
            throw new DomainException("User is already deactivated.");
        }

        IsActive = false;
    }

    /// <summary>
    /// Activates the user.
    /// </summary>
    public void Activate()
    {
        if (IsActive)
        {
            throw new DomainException("User is already active.");
        }

        IsActive = true;
    }

    /// <summary>
    /// Updates the user's name.
    /// </summary>
    /// <param name="name">New name value.</param>
    public void UpdateName(string name)
    {
        Name = ValueObjects.Name.Create(name);
    }

    /// <summary>
    /// Clears all domain events.
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    private void RaiseDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
