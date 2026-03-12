using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Usuario.Domain.Entities;
using Usuario.Domain.Interfaces;
using Usuario.Application.Validators;

namespace Usuario.Application.Commands;

public class CriarUsuarioCommandHandler : IRequestHandler<CriarUsuarioCommand, Result<Guid>>
{
    private readonly IUsuarioRepository _usuarioRepository;

    public CriarUsuarioCommandHandler(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<Result<Guid>> Handle(CriarUsuarioCommand request, CancellationToken cancellationToken)
    {
        var validator = new CriarUsuarioCommandValidator();
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            return Result<Guid>.Failure(validationResult.Errors);
        }

        if (await _usuarioRepository.CPFJaCadastradoAsync(request.CPF))
        {
            return Result<Guid>.Failure("CPF já cadastrado.");
        }

        if (await _usuarioRepository.EmailJaCadastradoAsync(request.Email))
        {
            return Result<Guid>.Failure("E-mail já cadastrado.");
        }

        var usuario = new Usuario(
            request.Nome,
            request.CPF,
            request.DataNascimento,
            request.Email,
            request.Profissao
        );

        await _usuarioRepository.AdicionarAsync(usuario);

        return Result<Guid>.Success(usuario.Id);
    }
}
