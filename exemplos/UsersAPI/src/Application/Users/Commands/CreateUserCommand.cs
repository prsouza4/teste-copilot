using MediatR;

namespace Application.Users.Commands;

public record CreateUserCommand(string Name, string Email) : IRequest<Guid>;
