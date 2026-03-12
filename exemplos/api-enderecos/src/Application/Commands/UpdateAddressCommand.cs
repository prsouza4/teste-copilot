using MediatR;

namespace ApiEnderecos.Application.Commands;

public record UpdateAddressCommand(Guid Id, string Street, string City, string State, string ZipCode) : IRequest;
