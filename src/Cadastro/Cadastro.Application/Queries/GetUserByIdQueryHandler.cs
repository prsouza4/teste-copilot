using Cadastro.Application.DTOs;
using Cadastro.Domain.Interfaces;
using MediatR;

namespace Cadastro.Application.Queries;

/// <summary>
/// Handler for GetUserByIdQuery.
/// </summary>
public sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);

        if (user is null)
        {
            return null;
        }

        return new UserDto(
            user.Id,
            user.Name.Value,
            user.Email.Value,
            user.CreatedAt,
            user.IsActive);
    }
}
