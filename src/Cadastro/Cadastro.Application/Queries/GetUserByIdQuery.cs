using Cadastro.Application.DTOs;
using MediatR;

namespace Cadastro.Application.Queries;

/// <summary>
/// Query to get a user by their ID.
/// </summary>
public sealed record GetUserByIdQuery(Guid Id) : IRequest<UserDto?>;
