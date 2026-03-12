using ApiEnderecos.Application.Commands;
using ApiEnderecos.Domain.Interfaces;
using MediatR;

namespace ApiEnderecos.Application.Handlers;

public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand>
{
    private readonly IAddressRepository _repository;

    public UpdateAddressCommandHandler(IAddressRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        var address = await _repository.GetByIdAsync(request.Id);
        if (address == null)
        {
            throw new KeyNotFoundException("Address not found.");
        }

        address.Update(request.Street, request.City, request.State, request.ZipCode);
        await _repository.UpdateAsync(address);

        return Unit.Value;
    }
}
