using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace VendeTudo.Identidade.API;

/// <summary>
/// Serviço hospedado para semeadura de dados iniciais.
/// </summary>
public class SemeadorDadosIdentidade : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SemeadorDadosIdentidade> _logger;

    public SemeadorDadosIdentidade(IServiceProvider serviceProvider, ILogger<SemeadorDadosIdentidade> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<IdentidadeDbContext>();
        // EnsureCreatedAsync é intencional nesta aplicação de referência.
        // Em produção, substitua por context.Database.MigrateAsync() para gerenciar migrations.
        await context.Database.EnsureCreatedAsync(cancellationToken);

        await SemearRolesAsync(scope.ServiceProvider, cancellationToken);
        await SemearUsuariosAsync(scope.ServiceProvider, cancellationToken);
        await SemearClientesOIDCAsync(scope.ServiceProvider, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private async Task SemearRolesAsync(IServiceProvider serviceProvider, CancellationToken ct)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        string[] roles = ["Admin", "Cliente"];

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
                _logger.LogInformation("Role {Role} criada", role);
            }
        }
    }

    private async Task SemearUsuariosAsync(IServiceProvider serviceProvider, CancellationToken ct)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<UsuarioAplicacao>>();
        var config = serviceProvider.GetRequiredService<IOptions<IdentidadeConfig>>().Value;

        foreach (var usuarioConfig in config.UsuariosIniciais)
        {
            var usuarioExistente = await userManager.FindByEmailAsync(usuarioConfig.Email);
            if (usuarioExistente is not null)
            {
                continue;
            }

            var usuario = new UsuarioAplicacao
            {
                UserName = usuarioConfig.UserName,
                Email = usuarioConfig.Email,
                EmailConfirmed = true
            };

            var resultado = await userManager.CreateAsync(usuario, usuarioConfig.Password);
            if (resultado.Succeeded)
            {
                await userManager.AddToRolesAsync(usuario, usuarioConfig.Roles);
                _logger.LogInformation("Usuário {Usuario} criado", usuarioConfig.UserName);
            }
            else
            {
                _logger.LogError("Falha ao criar usuário {Usuario}: {Erros}",
                    usuarioConfig.UserName, string.Join(", ", resultado.Errors.Select(e => e.Description)));
            }
        }
    }

    private async Task SemearClientesOIDCAsync(IServiceProvider serviceProvider, CancellationToken ct)
    {
        var manager = serviceProvider.GetRequiredService<IOpenIddictApplicationManager>();
        var config = serviceProvider.GetRequiredService<IOptions<IdentidadeConfig>>().Value;

        foreach (var clienteConfig in config.ClientesOIDC)
        {
            var clienteExistente = await manager.FindByClientIdAsync(clienteConfig.ClientId, ct);
            if (clienteExistente is not null)
            {
                continue;
            }

            var descriptor = new OpenIddictApplicationDescriptor
            {
                ClientId = clienteConfig.ClientId,
                ClientSecret = clienteConfig.ClientSecret,
                DisplayName = clienteConfig.DisplayName,
                ConsentType = ConsentTypes.Explicit,
                Permissions =
                {
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.Logout,
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.GrantTypes.RefreshToken,
                    Permissions.ResponseTypes.Code,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles
                }
            };

            foreach (var uri in clienteConfig.RedirectUris)
            {
                descriptor.RedirectUris.Add(new Uri(uri));
            }

            foreach (var uri in clienteConfig.PostLogoutRedirectUris)
            {
                descriptor.PostLogoutRedirectUris.Add(new Uri(uri));
            }

            await manager.CreateAsync(descriptor, ct);
            _logger.LogInformation("Cliente OIDC {ClientId} criado", clienteConfig.ClientId);
        }
    }
}
