using MediatR;

namespace ApiEnderecos.Application.Commands;

public record CreateAddressCommand(string Street, string City, string State, string ZipCode) : IRequest<Guid>;
