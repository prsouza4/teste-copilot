using MediatR;

namespace EnderecoApi.Application.Commands;

public record CreateEnderecoCommand(string Logradouro, string Numero, string Bairro, string Cidade, string Estado, string Cep) : IRequest<Guid>;
