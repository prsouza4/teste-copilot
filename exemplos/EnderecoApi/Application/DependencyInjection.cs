using EnderecoApi.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EnderecoApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<EnderecoService>();

        return services;
    }
}
