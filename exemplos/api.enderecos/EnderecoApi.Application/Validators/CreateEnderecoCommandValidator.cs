using EnderecoApi.Application.Commands;
using FluentValidation;

namespace EnderecoApi.Application.Validators;

public class CreateEnderecoCommandValidator : AbstractValidator<CreateEnderecoCommand>
{
    public CreateEnderecoCommandValidator()
    {
        RuleFor(x => x.Logradouro).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Numero).NotEmpty().MaximumLength(10);
        RuleFor(x => x.Bairro).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Cidade).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Estado).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Cep).NotEmpty().Matches(@"^\d{5}-\d{3}$");
    }
}
