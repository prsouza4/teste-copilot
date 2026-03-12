using ApiEnderecos.Application.DTOs;
using MediatR;

namespace ApiEnderecos.Application.Queries;

public record GetAllAddressesQuery() : IRequest<IEnumerable<AddressDto>>;
