using MediatR;
using UserApi.Domain.Entities;
using UserApi.Domain.Interfaces;

namespace UserApi.Application.Users.Commands;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User(request.Name, request.Email);
        await _userRepository.AddAsync(user);
        return user.Id;
    }
}
