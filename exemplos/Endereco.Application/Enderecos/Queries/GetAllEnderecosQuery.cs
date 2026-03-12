using MediatR;

namespace Endereco.Application.Enderecos.Queries;

public record GetAllEnderecosQuery : IRequest<IEnumerable<EnderecoDto>>;

public record EnderecoDto(Guid Id, string Rua, string Numero, string Cidade, string Estado, string CEP);
