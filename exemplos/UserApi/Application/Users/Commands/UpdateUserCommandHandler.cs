using MediatR;
using UserApi.Domain.Interfaces;

namespace UserApi.Application.Users.Commands;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {request.Id} not found.");
        }

        user.Update(request.Name, request.Email);
        await _userRepository.UpdateAsync(user);

        return Unit.Value;
    }
}
