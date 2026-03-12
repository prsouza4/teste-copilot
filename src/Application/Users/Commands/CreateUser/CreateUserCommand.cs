using MediatR;
using Usuario.Application.Common;

namespace Usuario.Application.Users.Commands.CreateUser
{
    public record CreateUserCommand(string Name, string CPF, DateTime DateOfBirth, string Profession, string Email) : IRequest<Result<Guid>>;
}
