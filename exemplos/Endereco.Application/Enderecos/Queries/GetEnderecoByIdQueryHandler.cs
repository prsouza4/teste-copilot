using Endereco.Domain.Interfaces;
using MediatR;

namespace Endereco.Application.Enderecos.Queries;

public class GetEnderecoByIdQueryHandler : IRequestHandler<GetEnderecoByIdQuery, EnderecoDto>
{
    private readonly IEnderecoRepository _repository;

    public GetEnderecoByIdQueryHandler(IEnderecoRepository repository)
    {
        _repository = repository;
    }

    public async Task<EnderecoDto> Handle(GetEnderecoByIdQuery request, CancellationToken cancellationToken)
    {
        var endereco = await _repository.GetByIdAsync(request.Id);
        return endereco is null ? null : new EnderecoDto(endereco.Id, endereco.Rua, endereco.Numero, endereco.Cidade, endereco.Estado, endereco.CEP);
    }
}
