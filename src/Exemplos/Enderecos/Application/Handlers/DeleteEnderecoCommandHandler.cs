using Exemplos.Enderecos.Application.Commands;
using Exemplos.Enderecos.Domain.Interfaces;
using MediatR;

namespace Exemplos.Enderecos.Application.Handlers;

public class DeleteEnderecoCommandHandler : IRequestHandler<DeleteEnderecoCommand>
{
    private readonly IEnderecoRepository _repository;

    public DeleteEnderecoCommandHandler(IEnderecoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteEnderecoCommand request, CancellationToken cancellationToken)
    {
        var endereco = await _repository.GetByIdAsync(request.Id);
        if (endereco == null)
        {
            throw new KeyNotFoundException("Endereço não encontrado.");
        }

        await _repository.DeleteAsync(endereco);
        return Unit.Value;
    }
}
