using MediatR;

namespace Cadastro.Application.Commands;

/// <summary>
/// Command to create a new user.
/// </summary>
public sealed record CreateUserCommand(
    string Name,
    string Email) : IRequest<Guid>;
