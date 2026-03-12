using EnderecoApi.Application.Interfaces;
using EnderecoApi.Application.Services;
using EnderecoApi.Domain.Entities;
using FluentAssertions;
using Moq;

namespace EnderecoApi.Tests.Services;

public class EnderecoServiceTests
{
    private readonly Mock<IEnderecoRepository> _repositoryMock;
    private readonly EnderecoService _service;

    public EnderecoServiceTests()
    {
        _repositoryMock = new Mock<IEnderecoRepository>();
        _service = new EnderecoService(_repositoryMock.Object);
    }

    [Fact]
    public async Task AddAsync_ShouldThrowException_WhenEnderecoAlreadyExists()
    {
        // Arrange
        var endereco = new Endereco("Rua A", "123", "Cidade B", "Estado C", "12345-678");
        _repositoryMock.Setup(r => r.ExistsAsync(endereco.Rua, endereco.Numero, endereco.Cidade, endereco.Estado, endereco.Cep))
            .ReturnsAsync(true);

        // Act
        var act = async () => await _service.AddAsync(endereco);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Endereço já cadastrado.");
    }

    [Fact]
    public async Task AddAsync_ShouldAddEndereco_WhenEnderecoDoesNotExist()
    {
        // Arrange
        var endereco = new Endereco("Rua A", "123", "Cidade B", "Estado C", "12345-678");
        _repositoryMock.Setup(r => r.ExistsAsync(endereco.Rua, endereco.Numero, endereco.Cidade, endereco.Estado, endereco.Cep))
            .ReturnsAsync(false);

        // Act
        await _service.AddAsync(endereco);

        // Assert
        _repositoryMock.Verify(r => r.AddAsync(endereco), Times.Once);
    }
}
