using MediatR;

namespace Endereco.Application.Enderecos.Commands;

public record DeleteEnderecoCommand(Guid Id) : IRequest<bool>;
