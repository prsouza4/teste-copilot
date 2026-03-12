using Duende.IdentityServer.Models;

namespace API.Auth;

/// <summary>
/// IdentityServer configuration with clients, API resources, and identity resources.
/// </summary>
public static class Config
{
    /// <summary>
    /// Gets the identity resources for OpenID Connect.
    /// </summary>
    public static IEnumerable<IdentityResource> GetIdentityResources() =>
    [
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResources.Email()
    ];

    /// <summary>
    /// Gets the API scopes for the cadastro API.
    /// </summary>
    public static IEnumerable<ApiScope> GetApiScopes() =>
    [
        new ApiScope("api.cadastro:read", "Read access to Cadastro API"),
        new ApiScope("api.cadastro:write", "Write access to Cadastro API")
    ];

    /// <summary>
    /// Gets the API resources.
    /// </summary>
    public static IEnumerable<ApiResource> GetApiResources() =>
    [
        new ApiResource("api.cadastro", "Cadastro API")
        {
            Scopes = { "api.cadastro:read", "api.cadastro:write" }
        }
    ];

    /// <summary>
    /// Gets the configured clients.
    /// </summary>
    public static IEnumerable<Client> GetClients() =>
    [
        new Client
        {
            ClientId = "nextjs-client",
            ClientName = "Next.js Frontend",
            RequireClientSecret = false,
            AllowedGrantTypes = GrantTypes.Code,
            RequirePkce = true,
            RedirectUris = { "http://localhost:3000/api/auth/callback/duende" },
            PostLogoutRedirectUris = { "http://localhost:3000" },
            AllowedCorsOrigins = { "http://localhost:3000" },
            AllowedScopes =
            {
                "openid",
                "profile",
                "email",
                "api.cadastro:read",
                "api.cadastro:write"
            },
            AllowOfflineAccess = true
        }
    ];
}
