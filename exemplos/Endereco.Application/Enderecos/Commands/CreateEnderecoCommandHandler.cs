using Endereco.Domain.Entities;
using Endereco.Domain.Interfaces;
using MediatR;

namespace Endereco.Application.Enderecos.Commands;

public class CreateEnderecoCommandHandler : IRequestHandler<CreateEnderecoCommand, Guid>
{
    private readonly IEnderecoRepository _repository;

    public CreateEnderecoCommandHandler(IEnderecoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateEnderecoCommand request, CancellationToken cancellationToken)
    {
        var exists = await _repository.ExistsAsync(request.Rua, request.Numero, request.Cidade, request.Estado, request.CEP);
        if (exists) throw new InvalidOperationException("Endereço já cadastrado.");

        var endereco = new Endereco(request.Rua, request.Numero, request.Cidade, request.Estado, request.CEP);
        await _repository.AddAsync(endereco);

        return endereco.Id;
    }
}
