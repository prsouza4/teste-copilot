using System;
using MediatR;

namespace Usuario.Application.Commands;

public record CriarUsuarioCommand(
    string Nome,
    string CPF,
    DateTime DataNascimento,
    string Email,
    string? Profissao
) : IRequest<Result<Guid>>;
