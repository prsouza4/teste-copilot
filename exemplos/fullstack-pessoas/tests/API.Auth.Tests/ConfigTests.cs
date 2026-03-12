using API.Auth;
using FluentAssertions;
using Xunit;

namespace API.Auth.Tests;

/// <summary>
/// Unit tests for the IdentityServer configuration.
/// </summary>
public class ConfigTests
{
    /// <summary>
    /// Tests that GetClients returns at least one client with ClientId = "nextjs-client".
    /// </summary>
    [Fact]
    public void GetClients_ReturnsNextJsClient()
    {
        // Arrange & Act
        var clients = Config.GetClients().ToList();

        // Assert
        clients.Should().NotBeEmpty();
        clients.Should().Contain(c => c.ClientId == "nextjs-client");
    }

    /// <summary>
    /// Tests that the nextjs-client has the correct configuration.
    /// </summary>
    [Fact]
    public void GetClients_NextJsClient_HasCorrectConfiguration()
    {
        // Arrange & Act
        var client = Config.GetClients().FirstOrDefault(c => c.ClientId == "nextjs-client");

        // Assert
        client.Should().NotBeNull();
        client!.RequireClientSecret.Should().BeFalse();
        client.RequirePkce.Should().BeTrue();
        client.RedirectUris.Should().Contain("http://localhost:3000/api/auth/callback/duende");
        client.PostLogoutRedirectUris.Should().Contain("http://localhost:3000");
        client.AllowedCorsOrigins.Should().Contain("http://localhost:3000");
    }

    /// <summary>
    /// Tests that GetApiScopes contains api.cadastro:read and api.cadastro:write.
    /// </summary>
    [Fact]
    public void GetApiScopes_ContainsReadAndWriteScopes()
    {
        // Arrange & Act
        var scopes = Config.GetApiScopes().ToList();

        // Assert
        scopes.Should().NotBeEmpty();
        scopes.Should().Contain(s => s.Name == "api.cadastro:read");
        scopes.Should().Contain(s => s.Name == "api.cadastro:write");
    }

    /// <summary>
    /// Tests that GetIdentityResources contains openid, profile, and email.
    /// </summary>
    [Fact]
    public void GetIdentityResources_ContainsRequiredResources()
    {
        // Arrange & Act
        var resources = Config.GetIdentityResources().ToList();

        // Assert
        resources.Should().NotBeEmpty();
        resources.Should().Contain(r => r.Name == "openid");
        resources.Should().Contain(r => r.Name == "profile");
        resources.Should().Contain(r => r.Name == "email");
    }

    /// <summary>
    /// Tests that GetApiResources contains api.cadastro with correct scopes.
    /// </summary>
    [Fact]
    public void GetApiResources_ContainsCadastroApi()
    {
        // Arrange & Act
        var resources = Config.GetApiResources().ToList();

        // Assert
        resources.Should().NotBeEmpty();
        var cadastroApi = resources.FirstOrDefault(r => r.Name == "api.cadastro");
        cadastroApi.Should().NotBeNull();
        cadastroApi!.Scopes.Should().Contain("api.cadastro:read");
        cadastroApi.Scopes.Should().Contain("api.cadastro:write");
    }

    /// <summary>
    /// Tests that the nextjs-client has the required scopes.
    /// </summary>
    [Fact]
    public void GetClients_NextJsClient_HasRequiredScopes()
    {
        // Arrange & Act
        var client = Config.GetClients().FirstOrDefault(c => c.ClientId == "nextjs-client");

        // Assert
        client.Should().NotBeNull();
        client!.AllowedScopes.Should().Contain("openid");
        client.AllowedScopes.Should().Contain("profile");
        client.AllowedScopes.Should().Contain("email");
        client.AllowedScopes.Should().Contain("api.cadastro:read");
        client.AllowedScopes.Should().Contain("api.cadastro:write");
    }
}
