using ApiEnderecos.Application.DTOs;
using MediatR;

namespace ApiEnderecos.Application.Queries;

public record GetAddressByIdQuery(Guid Id) : IRequest<AddressDto>;
