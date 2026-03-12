using MediatR;

namespace UserApi.Application.Users.Commands;

public record CreateUserCommand(string Name, string Email) : IRequest<Guid>;
