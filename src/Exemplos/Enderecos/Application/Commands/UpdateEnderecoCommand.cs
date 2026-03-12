using MediatR;

namespace Exemplos.Enderecos.Application.Commands;

public record UpdateEnderecoCommand(
    Guid Id,
    string Rua,
    string Numero,
    string Bairro,
    string Cidade,
    string Estado,
    string Cep
) : IRequest;
