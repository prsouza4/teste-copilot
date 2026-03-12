using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VendeTudo.Identidade.API;
using VendeTudo.PadroeServico;

var builder = WebApplication.CreateBuilder(args);

// Configuração
builder.Services.Configure<IdentidadeConfig>(builder.Configuration.GetSection("IdentidadeConfig"));

// Padrões de serviço
builder.AdicionarPadroesServico();

// Database
builder.Services.AddDbContext<IdentidadeDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("BancoIdentidade"));
    options.UseOpenIddict();
});

// Identity
builder.Services.AddIdentity<UsuarioAplicacao, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<IdentidadeDbContext>()
.AddDefaultTokenProviders();

// OpenIddict
builder.Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
            .UseDbContext<IdentidadeDbContext>();
    })
    .AddServer(options =>
    {
        options.SetAuthorizationEndpointUris("/connect/authorize")
            .SetLogoutEndpointUris("/connect/logout")
            .SetTokenEndpointUris("/connect/token")
            .SetUserinfoEndpointUris("/connect/userinfo");

        options.AllowAuthorizationCodeFlow()
            .AllowRefreshTokenFlow();

        options.RegisterScopes("openid", "profile", "email", "roles");

        options.AddDevelopmentEncryptionCertificate()
            .AddDevelopmentSigningCertificate();

        options.UseAspNetCore()
            .EnableAuthorizationEndpointPassthrough()
            .EnableLogoutEndpointPassthrough()
            .EnableTokenEndpointPassthrough()
            .EnableUserinfoEndpointPassthrough();
    })
    .AddValidation(options =>
    {
        options.UseLocalServer();
        options.UseAspNetCore();
    });

// Razor Pages para login/logout
builder.Services.AddRazorPages();

// Semeador de dados
builder.Services.AddHostedService<SemeadorDadosIdentidade>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapearEndpointsAutorizacao();
app.MapearEndpointsPadrao();

app.Run();

public partial class Program { }
