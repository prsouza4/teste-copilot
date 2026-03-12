using EnderecoApi.Application.Commands;
using EnderecoApi.Domain.Entities;
using EnderecoApi.Domain.Interfaces;
using MediatR;

namespace EnderecoApi.Application.Handlers;

public class CreateEnderecoCommandHandler : IRequestHandler<CreateEnderecoCommand, Guid>
{
    private readonly IEnderecoRepository _repository;

    public CreateEnderecoCommandHandler(IEnderecoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateEnderecoCommand request, CancellationToken cancellationToken)
    {
        var endereco = new Endereco(request.Logradouro, request.Numero, request.Bairro, request.Cidade, request.Estado, request.Cep);
        await _repository.AddAsync(endereco);
        return endereco.Id;
    }
}
