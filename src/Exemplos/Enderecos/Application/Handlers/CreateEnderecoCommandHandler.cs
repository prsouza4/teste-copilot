using Exemplos.Enderecos.Application.Commands;
using Exemplos.Enderecos.Domain.Entities;
using Exemplos.Enderecos.Domain.Interfaces;
using MediatR;

namespace Exemplos.Enderecos.Application.Handlers;

public class CreateEnderecoCommandHandler : IRequestHandler<CreateEnderecoCommand, Guid>
{
    private readonly IEnderecoRepository _repository;

    public CreateEnderecoCommandHandler(IEnderecoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateEnderecoCommand request, CancellationToken cancellationToken)
    {
        if (await _repository.ExistsAsync(request.Rua, request.Numero, request.Bairro, request.Cidade, request.Estado, request.Cep))
        {
            throw new InvalidOperationException("Endereço já cadastrado.");
        }

        var endereco = new Endereco(request.Rua, request.Numero, request.Bairro, request.Cidade, request.Estado, request.Cep);
        await _repository.AddAsync(endereco);
        return endereco.Id;
    }
}
