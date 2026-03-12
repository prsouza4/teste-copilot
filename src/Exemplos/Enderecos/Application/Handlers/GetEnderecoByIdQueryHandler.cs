using Exemplos.Enderecos.Application.DTOs;
using Exemplos.Enderecos.Application.Queries;
using Exemplos.Enderecos.Domain.Interfaces;
using MediatR;

namespace Exemplos.Enderecos.Application.Handlers;

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
        if (endereco == null)
        {
            throw new KeyNotFoundException("Endereço não encontrado.");
        }

        return new EnderecoDto(
            endereco.Id,
            endereco.Rua,
            endereco.Numero,
            endereco.Bairro,
            endereco.Cidade,
            endereco.Estado,
            endereco.Cep
        );
    }
}
