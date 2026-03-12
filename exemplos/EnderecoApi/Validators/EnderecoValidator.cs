using EnderecoApi.Models;
using FluentValidation;

namespace EnderecoApi.Validators;

public class EnderecoValidator : AbstractValidator<Endereco>
{
    public EnderecoValidator()
    {
        RuleFor(e => e.Logradouro).NotEmpty().MaximumLength(100);
        RuleFor(e => e.Numero).NotEmpty().MaximumLength(10);
        RuleFor(e => e.Bairro).NotEmpty().MaximumLength(50);
        RuleFor(e => e.Cidade).NotEmpty().MaximumLength(50);
        RuleFor(e => e.Estado).NotEmpty().Length(2);
        RuleFor(e => e.Cep).NotEmpty().Matches(@"^\d{5}-\d{3}$");
    }
}
