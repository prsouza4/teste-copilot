using EnderecoApi.Models;
using FluentValidation;

namespace EnderecoApi.Validators;

public class EnderecoValidator : AbstractValidator<Endereco>
{
    public EnderecoValidator()
    {
        RuleFor(e => e.Logradouro).NotEmpty().WithMessage("O logradouro é obrigatório.");
        RuleFor(e => e.Numero).NotEmpty().WithMessage("O número é obrigatório.");
        RuleFor(e => e.Cidade).NotEmpty().WithMessage("A cidade é obrigatória.");
        RuleFor(e => e.Estado).NotEmpty().WithMessage("O estado é obrigatório.");
        RuleFor(e => e.CEP).NotEmpty().Matches(@"^\d{5}-\d{3}$").WithMessage("O CEP deve estar no formato 00000-000.");
    }
}
