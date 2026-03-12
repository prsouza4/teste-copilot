using FluentValidation;
using MediatR;

namespace Endereco.Application.Enderecos.Commands;

public record UpdateEnderecoCommand(Guid Id, string Rua, string Numero, string Cidade, string Estado, string CEP) : IRequest<bool>;

public class UpdateEnderecoCommandValidator : AbstractValidator<UpdateEnderecoCommand>
{
    public UpdateEnderecoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Rua).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Numero).NotEmpty().MaximumLength(10);
        RuleFor(x => x.Cidade).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Estado).NotEmpty().MaximumLength(50);
        RuleFor(x => x.CEP).NotEmpty().Matches(@"^\d{5}-\d{3}$").WithMessage("CEP inválido.");
    }
}
