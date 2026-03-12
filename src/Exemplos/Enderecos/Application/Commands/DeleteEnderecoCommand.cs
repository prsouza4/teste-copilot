using MediatR;

namespace Exemplos.Enderecos.Application.Commands;

public record DeleteEnderecoCommand(Guid Id) : IRequest;
