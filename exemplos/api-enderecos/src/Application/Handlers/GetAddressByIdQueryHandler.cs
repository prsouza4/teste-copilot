using ApiEnderecos.Application.DTOs;
using ApiEnderecos.Application.Queries;
using ApiEnderecos.Domain.Interfaces;
using MediatR;

namespace ApiEnderecos.Application.Handlers;

public class GetAddressByIdQueryHandler : IRequestHandler<GetAddressByIdQuery, AddressDto>
{
    private readonly IAddressRepository _repository;

    public GetAddressByIdQueryHandler(IAddressRepository repository)
    {
        _repository = repository;
    }

    public async Task<AddressDto> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
    {
        var address = await _repository.GetByIdAsync(request.Id);
        if (address == null)
        {
            throw new KeyNotFoundException("Address not found.");
        }

        return new AddressDto(address.Id, address.Street, address.City, address.State, address.ZipCode);
    }
}
