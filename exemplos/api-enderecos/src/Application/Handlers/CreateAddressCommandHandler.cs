using ApiEnderecos.Application.Commands;
using ApiEnderecos.Domain.Entities;
using ApiEnderecos.Domain.Interfaces;
using MediatR;

namespace ApiEnderecos.Application.Handlers;

public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, Guid>
{
    private readonly IAddressRepository _repository;

    public CreateAddressCommandHandler(IAddressRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        var existingAddress = await _repository.GetByDetailsAsync(request.Street, request.City, request.State, request.ZipCode);
        if (existingAddress != null)
        {
            throw new InvalidOperationException("Address already exists.");
        }

        var address = new Address(request.Street, request.City, request.State, request.ZipCode);
        await _repository.AddAsync(address);
        return address.Id;
    }
}
