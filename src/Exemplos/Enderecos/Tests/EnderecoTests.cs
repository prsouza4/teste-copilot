using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Exemplos.Enderecos.Domain.Entities;
using Exemplos.Enderecos.Domain.Interfaces;
using Exemplos.Enderecos.Application.Commands;
using Exemplos.Enderecos.Application.Handlers;

namespace Exemplos.Enderecos.Tests;

public class EnderecoTests
{
    [Fact]
    public async Task CreateEnderecoCommandHandler_Should_Create_New_Endereco()
    {
        // Arrange
        var repositoryMock = new Mock<IEnderecoRepository>();
        repositoryMock.Setup(r => r.ExistsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                      .ReturnsAsync(false);

        var handler = new CreateEnderecoCommandHandler(repositoryMock.Object);
        var command = new CreateEnderecoCommand("Rua A", "123", "Bairro B", "Cidade C", "SP", "12345678");

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        repositoryMock.Verify(r => r.AddAsync(It.IsAny<Endereco>()), Times.Once);
    }

    [Fact]
    public async Task CreateEnderecoCommandHandler_Should_Throw_Exception_For_Duplicate_Endereco()
    {
        // Arrange
        var repositoryMock = new Mock<IEnderecoRepository>();
        repositoryMock.Setup(r => r.ExistsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                      .ReturnsAsync(true);

        var handler = new CreateEnderecoCommandHandler(repositoryMock.Object);
        var command = new CreateEnderecoCommand("Rua A", "123", "Bairro B", "Cidade C", "SP", "12345678");

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, default));
    }
}
