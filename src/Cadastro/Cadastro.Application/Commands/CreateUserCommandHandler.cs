using Cadastro.Application.Interfaces;
using Cadastro.Domain.Entities;
using Cadastro.Domain.Exceptions;
using Cadastro.Domain.Interfaces;
using MediatR;

namespace Cadastro.Application.Commands;

/// <summary>
/// Handler for CreateUserCommand.
/// </summary>
public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IEventPublisher _eventPublisher;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IEventPublisher eventPublisher)
    {
        _userRepository = userRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Check if email is already in use
        var existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingUser is not null)
        {
            throw new DomainException($"A user with email '{request.Email}' already exists.");
        }

        // Create user aggregate
        var user = User.Create(request.Name, request.Email);

        // Persist user
        await _userRepository.AddAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);

        // Publish domain events
        foreach (var domainEvent in user.DomainEvents)
        {
            await _eventPublisher.PublishAsync(domainEvent, cancellationToken);
        }

        user.ClearDomainEvents();

        return user.Id;
    }
}
