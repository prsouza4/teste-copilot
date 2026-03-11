using MediatR;

namespace UserApi.Application.Users.Commands;

public record UpdateUserCommand(Guid Id, string Name, string Email) : IRequest;
