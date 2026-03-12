using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Encodings.Web;
using API.Cadastro.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xunit;

namespace API.Cadastro.Tests;

/// <summary>
/// Integration tests for the PessoasController.
/// </summary>
public class PessoasControllerTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly TestWebApplicationFactory _factory;

    public PessoasControllerTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// Tests that GET /api/pessoas returns 401 without a token.
    /// </summary>
    [Fact]
    public async Task GetPessoas_WithoutToken_Returns401()
    {
        // Arrange
        var client = _factory.CreateDefaultClient();

        // Act
        var response = await client.GetAsync("/api/pessoas");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests that GET /api/pessoas returns 200 with valid mock authentication.
    /// </summary>
    [Fact]
    public async Task GetPessoas_WithValidAuth_Returns200()
    {
        // Arrange
        var client = _factory.CreateAuthenticatedClient("api.cadastro:read");

        // Act
        var response = await client.GetAsync("/api/pessoas");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var pessoas = await response.Content.ReadFromJsonAsync<List<Pessoa>>();
        pessoas.Should().NotBeNull();
        pessoas!.Count.Should().BeGreaterThanOrEqualTo(3); // Seeded data
    }

    /// <summary>
    /// Tests that POST /api/pessoas with valid data returns 201.
    /// </summary>
    [Fact]
    public async Task CreatePessoa_WithValidData_Returns201()
    {
        // Arrange
        var client = _factory.CreateAuthenticatedClient("api.cadastro:write");
        var newPessoa = new PessoaDto
        {
            Nome = "Test User",
            Email = "test@example.com",
            Telefone = "(11) 99999-9999"
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/pessoas", newPessoa);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdPessoa = await response.Content.ReadFromJsonAsync<Pessoa>();
        createdPessoa.Should().NotBeNull();
        createdPessoa!.Nome.Should().Be("Test User");
        createdPessoa.Email.Should().Be("test@example.com");
        createdPessoa.Id.Should().NotBeEmpty();
    }

    /// <summary>
    /// Tests that PUT /api/pessoas/{id} returns 200.
    /// </summary>
    [Fact]
    public async Task UpdatePessoa_WithValidData_Returns200()
    {
        // Arrange
        var client = _factory.CreateAuthenticatedClient("api.cadastro:write api.cadastro:read");
        
        // First, create a pessoa
        var newPessoa = new PessoaDto
        {
            Nome = "Original Name",
            Email = "original@example.com",
            Telefone = "(11) 00000-0000"
        };
        var createResponse = await client.PostAsJsonAsync("/api/pessoas", newPessoa);
        var createdPessoa = await createResponse.Content.ReadFromJsonAsync<Pessoa>();

        // Act - Update the pessoa
        var updateDto = new PessoaDto
        {
            Nome = "Updated Name",
            Email = "updated@example.com",
            Telefone = "(11) 11111-1111"
        };
        var response = await client.PutAsJsonAsync($"/api/pessoas/{createdPessoa!.Id}", updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var updatedPessoa = await response.Content.ReadFromJsonAsync<Pessoa>();
        updatedPessoa.Should().NotBeNull();
        updatedPessoa!.Nome.Should().Be("Updated Name");
        updatedPessoa.Email.Should().Be("updated@example.com");
    }

    /// <summary>
    /// Tests that DELETE /api/pessoas/{id} returns 204.
    /// </summary>
    [Fact]
    public async Task DeletePessoa_WithValidId_Returns204()
    {
        // Arrange
        var client = _factory.CreateAuthenticatedClient("api.cadastro:write api.cadastro:read");
        
        // First, create a pessoa to delete
        var newPessoa = new PessoaDto
        {
            Nome = "To Be Deleted",
            Email = "delete@example.com",
            Telefone = "(11) 22222-2222"
        };
        var createResponse = await client.PostAsJsonAsync("/api/pessoas", newPessoa);
        var createdPessoa = await createResponse.Content.ReadFromJsonAsync<Pessoa>();

        // Act
        var response = await client.DeleteAsync($"/api/pessoas/{createdPessoa!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify the pessoa is deleted
        var getResponse = await client.GetAsync($"/api/pessoas/{createdPessoa.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Tests that GET /api/pessoas/{id} returns 404 for non-existent pessoa.
    /// </summary>
    [Fact]
    public async Task GetPessoa_WithNonExistentId_Returns404()
    {
        // Arrange
        var client = _factory.CreateAuthenticatedClient("api.cadastro:read");

        // Act
        var response = await client.GetAsync($"/api/pessoas/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Tests that POST /api/pessoas without write scope returns 403.
    /// </summary>
    [Fact]
    public async Task CreatePessoa_WithoutWriteScope_Returns403()
    {
        // Arrange - only read scope
        var client = _factory.CreateAuthenticatedClient("api.cadastro:read");
        var newPessoa = new PessoaDto
        {
            Nome = "Test User",
            Email = "test@example.com",
            Telefone = "(11) 99999-9999"
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/pessoas", newPessoa);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}

/// <summary>
/// Custom WebApplicationFactory for testing with mock authentication.
/// </summary>
public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        
        builder.ConfigureServices(services =>
        {
            // Add test authentication scheme
            services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
        });
    }

    /// <summary>
    /// Creates an HTTP client with test authentication.
    /// </summary>
    public HttpClient CreateAuthenticatedClient(string scopes)
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Add("X-Test-Scopes", scopes);
        client.DefaultRequestHeaders.Add("Authorization", "Bearer test-token");
        return client;
    }
}

/// <summary>
/// Test authentication handler that simulates JWT bearer authentication.
/// </summary>
public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder) : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Check if we have an Authorization header
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return Task.FromResult(AuthenticateResult.Fail("No Authorization header"));
        }

        // Get scopes from custom header
        var scopes = Request.Headers["X-Test-Scopes"].ToString();
        
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, "testuser"),
            new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new("sub", "testuser"),
        };

        // Add scope claims
        if (!string.IsNullOrEmpty(scopes))
        {
            claims.Add(new Claim("scope", scopes));
        }

        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
