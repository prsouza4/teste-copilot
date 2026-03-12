using EnderecoApi.Application.Commands;
using EnderecoApi.Application.Handlers;
using EnderecoApi.Domain.Entities;
using EnderecoApi.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace EnderecoApi.Tests.Handlers;

public class CreateEnderecoCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateEndereco()
    {
        var repositoryMock = new Mock<IEnderecoRepository>();
        var handler = new CreateEnderecoCommandHandler(repositoryMock.Object);

        var command = new CreateEnderecoCommand("Rua A", "123", "Bairro B", "Cidade C", "Estado D", "12345-678");
        var result = await handler.Handle(command, CancellationToken.None);

        repositoryMock.Verify(r => r.AddAsync(It.IsAny<Endereco>()), Times.Once);
        result.Should().NotBeEmpty();
    }
}
