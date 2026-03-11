using MediatR;
using UserApi.Application.Users.DTOs;

namespace UserApi.Application.Users.Queries;

public record GetUserByIdQuery(Guid Id) : IRequest<UserDto>;
