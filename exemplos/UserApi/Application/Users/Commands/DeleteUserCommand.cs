using MediatR;

namespace UserApi.Application.Users.Commands;

public record DeleteUserCommand(Guid Id) : IRequest;
