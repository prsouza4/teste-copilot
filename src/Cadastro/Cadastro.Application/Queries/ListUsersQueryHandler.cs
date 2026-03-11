using Cadastro.Application.DTOs;
using Cadastro.Domain.Interfaces;
using MediatR;

namespace Cadastro.Application.Queries;

/// <summary>
/// Handler for ListUsersQuery.
/// </summary>
public sealed class ListUsersQueryHandler : IRequestHandler<ListUsersQuery, PaginatedResponse<UserDto>>
{
    private readonly IUserRepository _userRepository;

    public ListUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<PaginatedResponse<UserDto>> Handle(ListUsersQuery request, CancellationToken cancellationToken)
    {
        var page = Math.Max(1, request.Page);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);

        var (users, totalCount) = await _userRepository.GetAllAsync(page, pageSize, cancellationToken);

        var userDtos = users.Select(u => new UserDto(
            u.Id,
            u.Name.Value,
            u.Email.Value,
            u.CreatedAt,
            u.IsActive));

        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        return new PaginatedResponse<UserDto>(
            userDtos,
            page,
            pageSize,
            totalCount,
            totalPages);
    }
}
