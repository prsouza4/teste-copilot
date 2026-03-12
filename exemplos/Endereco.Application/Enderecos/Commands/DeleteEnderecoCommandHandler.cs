using Endereco.Domain.Interfaces;
using MediatR;

namespace Endereco.Application.Enderecos.Commands;

public class DeleteEnderecoCommandHandler : IRequestHandler<DeleteEnderecoCommand, bool>
{
    private readonly IEnderecoRepository _repository;

    public DeleteEnderecoCommandHandler(IEnderecoRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteEnderecoCommand request, CancellationToken cancellationToken)
    {
        var endereco = await _repository.GetByIdAsync(request.Id);
        if (endereco is null) return false;

        await _repository.DeleteAsync(endereco);
        return true;
    }
}
