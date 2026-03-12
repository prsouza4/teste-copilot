using MediatR;

namespace Endereco.Application.Enderecos.Queries;

public record GetEnderecoByIdQuery(Guid Id) : IRequest<EnderecoDto>;
