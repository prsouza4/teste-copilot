using FluentValidation;
using Usuario.Application.Commands;

namespace Usuario.Application.Validators;

public class CriarUsuarioCommandValidator : AbstractValidator<CriarUsuarioCommand>
{
    public CriarUsuarioCommandValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.");

        RuleFor(x => x.CPF)
            .NotEmpty().WithMessage("O CPF é obrigatório.")
            .Must(ValidarCPF).WithMessage("O CPF informado é inválido.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("O e-mail informado é inválido.");

        RuleFor(x => x.DataNascimento)
            .NotEmpty().WithMessage("A data de nascimento é obrigatória.")
            .LessThan(DateTime.Now).WithMessage("A data de nascimento deve ser no passado.");
    }

    private bool ValidarCPF(string cpf)
    {
        // Implementar validação de CPF aqui
        return true;
    }
}
