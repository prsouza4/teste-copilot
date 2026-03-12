using AddressApi.Application.Commands;
using AddressApi.Application.Handlers;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace AddressApi.Tests.Handlers;

public class CreateAddressCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnNewAddressId()
    {
        var handler = new CreateAddressCommandHandler();
        var command = new CreateAddressCommand("Street", "City", "State", "PostalCode");

        var result = await handler.Handle(command, default);

        result.Should().NotBeEmpty();
    }
}
