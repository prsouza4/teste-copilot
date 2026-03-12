using EnderecoApi.Controllers;
using EnderecoApi.Data;
using EnderecoApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EnderecoApi.Tests;

public class EnderecoControllerTests
{
    private readonly EnderecoDbContext _context;
    private readonly EnderecoController _controller;

    public EnderecoControllerTests()
    {
        var options = new DbContextOptionsBuilder<EnderecoDbContext>()
            .UseInMemoryDatabase(databaseName: "EnderecoTestDb")
            .Options;

        _context = new EnderecoDbContext(options);
        _controller = new EnderecoController(_context);
    }

    [Fact]
    public async Task CreateEndereco_ShouldAddNewEndereco()
    {
        var endereco = new Endereco
        {
            Logradouro = "Rua A",
            Numero = "123",
            Complemento = "Apto 1",
            Bairro = "Centro",
            Cidade = "São Paulo",
            Estado = "SP",
            CEP = "12345-678"
        };

        var result = await _controller.CreateEndereco(endereco);

        result.Should().BeOfType<CreatedAtActionResult>();
        var createdEndereco = (result as CreatedAtActionResult)?.Value as Endereco;
        createdEndereco.Should().NotBeNull();
        createdEndereco?.Logradouro.Should().Be("Rua A");
    }

    [Fact]
    public async Task CreateEndereco_ShouldNotAllowDuplicateEndereco()
    {
        var endereco = new Endereco
        {
            Logradouro = "Rua A",
            Numero = "123",
            Complemento = "Apto 1",
            Bairro = "Centro",
            Cidade = "São Paulo",
            Estado = "SP",
            CEP = "12345-678"
        };

        await _controller.CreateEndereco(endereco);
        var result = await _controller.CreateEndereco(endereco);

        result.Should().BeOfType<ConflictObjectResult>();
    }
}
