using Exemplos.Enderecos.Application.DTOs;
using MediatR;

namespace Exemplos.Enderecos.Application.Queries;

public record GetAllEnderecosQuery : IRequest<IEnumerable<EnderecoDto>>;
