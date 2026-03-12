using Exemplos.Enderecos.Application.Commands;
using Exemplos.Enderecos.Domain.Interfaces;
using MediatR;

namespace Exemplos.Enderecos.Application.Handlers;

public class UpdateEnderecoCommandHandler : IRequestHandler<UpdateEnderecoCommand>
{
    private readonly IEnderecoRepository _repository;

    public UpdateEnderecoCommandHandler(IEnderecoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(UpdateEnderecoCommand request, CancellationToken cancellationToken)
    {
        var endereco = await _repository.GetByIdAsync(request.Id);
        if (endereco == null)
        {
            throw new KeyNotFoundException("Endereço não encontrado.");
        }

        endereco.Update(request.Rua, request.Numero, request.Bairro, request.Cidade, request.Estado, request.Cep);
        await _repository.UpdateAsync(endereco);
        return Unit.Value;
    }
}
