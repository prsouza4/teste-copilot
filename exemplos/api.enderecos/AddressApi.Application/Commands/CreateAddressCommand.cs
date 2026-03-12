using MediatR;

namespace AddressApi.Application.Commands;

public record CreateAddressCommand(string Street, string City, string State, string PostalCode) : IRequest<Guid>;
