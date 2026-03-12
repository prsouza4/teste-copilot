using Exemplos.Enderecos.Application.DTOs;
using Exemplos.Enderecos.Application.Queries;
using Exemplos.Enderecos.Domain.Interfaces;
using MediatR;

namespace Exemplos.Enderecos.Application.Handlers;

public class GetAllEnderecosQueryHandler : IRequestHandler<GetAllEnderecosQuery, IEnumerable<EnderecoDto>>
{
    private readonly IEnderecoRepository _repository;

    public GetAllEnderecosQueryHandler(IEnderecoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<EnderecoDto>> Handle(GetAllEnderecosQuery request, CancellationToken cancellationToken)
    {
        var enderecos = await _repository.GetAllAsync();
        return enderecos.Select(e => new EnderecoDto(
            e.Id,
            e.Rua,
            e.Numero,
            e.Bairro,
            e.Cidade,
            e.Estado,
            e.Cep
        ));
    }
}
