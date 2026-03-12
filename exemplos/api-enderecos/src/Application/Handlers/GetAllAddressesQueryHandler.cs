using ApiEnderecos.Application.DTOs;
using ApiEnderecos.Application.Queries;
using ApiEnderecos.Domain.Interfaces;
using MediatR;

namespace ApiEnderecos.Application.Handlers;

public class GetAllAddressesQueryHandler : IRequestHandler<GetAllAddressesQuery, IEnumerable<AddressDto>>
{
    private readonly IAddressRepository _repository;

    public GetAllAddressesQueryHandler(IAddressRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AddressDto>> Handle(GetAllAddressesQuery request, CancellationToken cancellationToken)
    {
        var addresses = await _repository.GetAllAsync();
        return addresses.Select(a => new AddressDto(a.Id, a.Street, a.City, a.State, a.ZipCode));
    }
}
