using System.Security.Claims;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace VendeTudo.Identidade.API;

/// <summary>
/// Endpoints de autorização OpenID Connect.
/// </summary>
public static class EndpointsAutorizacao
{
    public static void MapearEndpointsAutorizacao(this WebApplication app)
    {
        app.MapGet("/connect/authorize", Autorizar);
        app.MapPost("/connect/authorize", Autorizar);
        app.MapPost("/connect/token", Token);
        app.MapGet("/connect/logout", Logout);
        app.MapPost("/connect/logout", Logout);
        app.MapGet("/connect/userinfo", UserInfo);
    }

    private static async Task<IResult> Autorizar(
        HttpContext context,
        IOpenIddictApplicationManager applicationManager,
        IOpenIddictScopeManager scopeManager,
        UserManager<UsuarioAplicacao> userManager,
        SignInManager<UsuarioAplicacao> signInManager)
    {
        var request = context.GetOpenIddictServerRequest()
            ?? throw new InvalidOperationException("A solicitação OpenID Connect não pode ser recuperada.");

        var result = await context.AuthenticateAsync(IdentityConstants.ApplicationScheme);

        if (!result.Succeeded || request.HasPrompt(Prompts.Login))
        {
            if (request.HasPrompt(Prompts.None))
            {
                return Results.Forbid(
                    new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.LoginRequired,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "O usuário não está logado."
                    }),
                    [OpenIddictServerAspNetCoreDefaults.AuthenticationScheme]);
            }

            var prompt = string.Join(" ", request.GetPrompts().Remove(Prompts.Login));
            var parameters = context.Request.HasFormContentType
                ? context.Request.Form.Where(p => p.Key != Parameters.Prompt).ToList()
                : context.Request.Query.Where(p => p.Key != Parameters.Prompt).ToList();

            parameters.Add(KeyValuePair.Create(Parameters.Prompt, new Microsoft.Extensions.Primitives.StringValues(prompt)));

            var redirectUri = context.Request.PathBase + context.Request.Path +
                QueryString.Create(parameters);

            return Results.Challenge(
                new AuthenticationProperties { RedirectUri = redirectUri },
                [IdentityConstants.ApplicationScheme]);
        }

        var user = await userManager.GetUserAsync(result.Principal)
            ?? throw new InvalidOperationException("O usuário não pode ser recuperado.");

        var application = await applicationManager.FindByClientIdAsync(request.ClientId!)
            ?? throw new InvalidOperationException("Detalhes da aplicação não encontrados.");

        var identity = new ClaimsIdentity(
            authenticationType: TokenValidationParameters.DefaultAuthenticationType,
            nameType: Claims.Name,
            roleType: Claims.Role);

        identity.SetClaim(Claims.Subject, await userManager.GetUserIdAsync(user))
            .SetClaim(Claims.Email, await userManager.GetEmailAsync(user))
            .SetClaim(Claims.Name, await userManager.GetUserNameAsync(user));

        var roles = await userManager.GetRolesAsync(user);
        identity.SetClaims(Claims.Role, [.. roles]);

        identity.SetScopes(request.GetScopes());

        identity.SetResources(await scopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync());

        identity.SetDestinations(GetDestinations);

        return Results.SignIn(new ClaimsPrincipal(identity),
            new AuthenticationProperties(),
            OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private static async Task<IResult> Token(
        HttpContext context,
        UserManager<UsuarioAplicacao> userManager,
        SignInManager<UsuarioAplicacao> signInManager,
        IOpenIddictScopeManager scopeManager)
    {
        var request = context.GetOpenIddictServerRequest()
            ?? throw new InvalidOperationException("A solicitação OpenID Connect não pode ser recuperada.");

        if (request.IsAuthorizationCodeGrantType() || request.IsRefreshTokenGrantType())
        {
            var result = await context.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            var user = await userManager.FindByIdAsync(result.Principal?.GetClaim(Claims.Subject)!);
            if (user is null)
            {
                return Results.Forbid(
                    new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "O token não é mais válido."
                    }),
                    [OpenIddictServerAspNetCoreDefaults.AuthenticationScheme]);
            }

            if (!await signInManager.CanSignInAsync(user))
            {
                return Results.Forbid(
                    new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "O usuário não tem mais permissão para fazer login."
                    }),
                    [OpenIddictServerAspNetCoreDefaults.AuthenticationScheme]);
            }

            var identity = new ClaimsIdentity(result.Principal!.Claims,
                authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                nameType: Claims.Name,
                roleType: Claims.Role);

            identity.SetClaim(Claims.Subject, await userManager.GetUserIdAsync(user))
                .SetClaim(Claims.Email, await userManager.GetEmailAsync(user))
                .SetClaim(Claims.Name, await userManager.GetUserNameAsync(user));

            var roles = await userManager.GetRolesAsync(user);
            identity.SetClaims(Claims.Role, [.. roles]);

            identity.SetDestinations(GetDestinations);

            return Results.SignIn(new ClaimsPrincipal(identity),
                new AuthenticationProperties(),
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        throw new InvalidOperationException("O tipo de concessão especificado não é suportado.");
    }

    private static async Task<IResult> Logout(HttpContext context, SignInManager<UsuarioAplicacao> signInManager)
    {
        await signInManager.SignOutAsync();

        return Results.SignOut(
            new AuthenticationProperties { RedirectUri = "/" },
            [OpenIddictServerAspNetCoreDefaults.AuthenticationScheme]);
    }

    private static async Task<IResult> UserInfo(HttpContext context, UserManager<UsuarioAplicacao> userManager)
    {
        var result = await context.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        var user = await userManager.FindByIdAsync(result.Principal?.GetClaim(Claims.Subject)!);

        if (user is null)
        {
            return Results.Challenge(
                new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidToken,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "O token de acesso especificado está vinculado a uma conta que não existe mais."
                }),
                [OpenIddictServerAspNetCoreDefaults.AuthenticationScheme]);
        }

        var claims = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            [Claims.Subject] = await userManager.GetUserIdAsync(user)
        };

        var principal = result.Principal;
        if (principal is not null && principal.HasScope(Scopes.Email))
        {
            claims[Claims.Email] = (await userManager.GetEmailAsync(user))!;
            claims[Claims.EmailVerified] = await userManager.IsEmailConfirmedAsync(user);
        }

        if (principal is not null && principal.HasScope(Scopes.Profile))
        {
            claims[Claims.Name] = (await userManager.GetUserNameAsync(user))!;
        }

        if (principal is not null && principal.HasScope(Scopes.Roles))
        {
            claims[Claims.Role] = await userManager.GetRolesAsync(user);
        }

        return Results.Ok(claims);
    }

    private static IEnumerable<string> GetDestinations(Claim claim)
    {
        switch (claim.Type)
        {
            case Claims.Name:
                yield return Destinations.AccessToken;
                if (claim.Subject?.HasScope(Scopes.Profile) == true)
                    yield return Destinations.IdentityToken;
                yield break;

            case Claims.Email:
                yield return Destinations.AccessToken;
                if (claim.Subject?.HasScope(Scopes.Email) == true)
                    yield return Destinations.IdentityToken;
                yield break;

            case Claims.Role:
                yield return Destinations.AccessToken;
                if (claim.Subject?.HasScope(Scopes.Roles) == true)
                    yield return Destinations.IdentityToken;
                yield break;

            case "AspNet.Identity.SecurityStamp":
                yield break;

            default:
                yield return Destinations.AccessToken;
                yield break;
        }
    }
}
