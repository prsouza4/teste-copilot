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
    public async Task GetEnderecos_ReturnsEmptyList_WhenNoEnderecosExist()
    {
        var result = await _controller.GetEnderecos();

        var okResult = Assert.IsType<ActionResult<IEnumerable<Endereco>>>(result);
        Assert.Empty(okResult.Value);
    }

    [Fact]
    public async Task CreateEndereco_ReturnsCreatedEndereco_WhenValid()
    {
        var endereco = new Endereco
        {
            Logradouro = "Rua A",
            Numero = "123",
            Bairro = "Centro",
            Cidade = "Cidade A",
            Estado = "SP",
            Cep = "12345-678"
        };

        var result = await _controller.CreateEndereco(endereco);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdEndereco = Assert.IsType<Endereco>(createdResult.Value);

        Assert.Equal(endereco.Logradouro, createdEndereco.Logradouro);
    }

    [Fact]
    public async Task CreateEndereco_ReturnsConflict_WhenEnderecoExists()
    {
        var endereco = new Endereco
        {
            Logradouro = "Rua A",
            Numero = "123",
            Bairro = "Centro",
            Cidade = "Cidade A",
            Estado = "SP",
            Cep = "12345-678"
        };

        _context.Enderecos.Add(endereco);
        await _context.SaveChangesAsync();

        var duplicateEndereco = new Endereco
        {
            Logradouro = "Rua A",
            Numero = "123",
            Bairro = "Centro",
            Cidade = "Cidade A",
            Estado = "SP",
            Cep = "12345-678"
        };

        var result = await _controller.CreateEndereco(duplicateEndereco);

        Assert.IsType<ConflictObjectResult>(result.Result);
    }
}
