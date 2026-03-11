using MediatR;
using UserApi.Application.Users.DTOs;
using UserApi.Domain.Interfaces;

namespace UserApi.Application.Users.Queries;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {request.Id} not found.");
        }

        return new UserDto(user.Id, user.Name, user.Email);
    }
}
