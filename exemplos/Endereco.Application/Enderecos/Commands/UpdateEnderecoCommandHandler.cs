using Endereco.Domain.Interfaces;
using MediatR;

namespace Endereco.Application.Enderecos.Commands;

public class UpdateEnderecoCommandHandler : IRequestHandler<UpdateEnderecoCommand, bool>
{
    private readonly IEnderecoRepository _repository;

    public UpdateEnderecoCommandHandler(IEnderecoRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(UpdateEnderecoCommand request, CancellationToken cancellationToken)
    {
        var endereco = await _repository.GetByIdAsync(request.Id);
        if (endereco is null) return false;

        endereco.Update(request.Rua, request.Numero, request.Cidade, request.Estado, request.CEP);
        await _repository.UpdateAsync(endereco);

        return true;
    }
}
