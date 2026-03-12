using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Usuario.Application.Common;
using Usuario.Domain.Entities;
using Usuario.Domain.Interfaces;

namespace Usuario.Application.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<Guid>>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User(Guid.NewGuid(), request.Name, request.CPF, request.DateOfBirth, request.Profession, request.Email);

            await _userRepository.AddAsync(user);

            return Result<Guid>.Success(user.Id);
        }
    }
}
