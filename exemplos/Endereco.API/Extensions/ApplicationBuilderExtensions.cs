namespace Endereco.API;

public static class ApplicationBuilderExtensions
{
    public static WebApplication UseApiConfiguration(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.MapEndpoints();

        return app;
    }

    private static void MapEndpoints(this WebApplication app)
    {
        app.MapEnderecoEndpoints();
    }
}
