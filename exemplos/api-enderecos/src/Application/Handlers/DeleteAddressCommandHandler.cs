using ApiEnderecos.Application.Commands;
using ApiEnderecos.Domain.Interfaces;
using MediatR;

namespace ApiEnderecos.Application.Handlers;

public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand>
{
    private readonly IAddressRepository _repository;

    public DeleteAddressCommandHandler(IAddressRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
        var address = await _repository.GetByIdAsync(request.Id);
        if (address == null)
        {
            throw new KeyNotFoundException("Address not found.");
        }

        await _repository.DeleteAsync(address);
        return Unit.Value;
    }
}
