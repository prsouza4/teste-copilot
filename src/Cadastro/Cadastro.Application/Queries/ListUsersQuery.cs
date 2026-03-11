using Cadastro.Application.DTOs;
using MediatR;

namespace Cadastro.Application.Queries;

/// <summary>
/// Query to list users with pagination.
/// </summary>
public sealed record ListUsersQuery(int Page = 1, int PageSize = 10) : IRequest<PaginatedResponse<UserDto>>;
