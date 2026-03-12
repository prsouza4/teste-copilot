using MediatR;

namespace Application.Users.Queries;

public record GetUserByIdQuery(Guid Id) : IRequest<UserDto>;

public record UserDto(Guid Id, string Name, string Email);
