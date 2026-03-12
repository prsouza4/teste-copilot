using AddressApi.Domain.Entities;
using MediatR;

namespace AddressApi.Application.Handlers;

public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, Guid>
{
    public Task<Guid> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        var address = new Address(request.Street, request.City, request.State, request.PostalCode);
        // Save to database (not implemented here)
        return Task.FromResult(address.Id);
    }
}
