using MediatR;

namespace ApiEnderecos.Application.Commands;

public record DeleteAddressCommand(Guid Id) : IRequest;
