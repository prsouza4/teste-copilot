using Exemplos.Enderecos.Application.DTOs;
using MediatR;

namespace Exemplos.Enderecos.Application.Queries;

public record GetEnderecoByIdQuery(Guid Id) : IRequest<EnderecoDto>;
