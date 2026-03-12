using Endereco.Domain.Interfaces;
using Endereco.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Endereco.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<EnderecoDbContext>(options =>
            options.UseInMemoryDatabase("EnderecoDb"));

        services.AddScoped<IEnderecoRepository, EnderecoRepository>();

        return services;
    }
}
