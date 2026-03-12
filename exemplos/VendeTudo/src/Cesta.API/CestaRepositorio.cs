using System.Text.Json;
using StackExchange.Redis;

namespace VendeTudo.Cesta.API;

/// <summary>
/// Interface do repositório de cestas.
/// </summary>
public interface ICestaRepositorio
{
    Task<CestaCliente?> ObterCestaAsync(string idCliente);
    Task<CestaCliente?> AtualizarCestaAsync(CestaCliente cesta);
    Task<bool> RemoverCestaAsync(string idCliente);
}

/// <summary>
/// Implementação do repositório de cestas usando Redis.
/// </summary>
public class CestaRepositorio : ICestaRepositorio
{
    private readonly IDatabase _database;
    private readonly ILogger<CestaRepositorio> _logger;

    public CestaRepositorio(IConnectionMultiplexer redis, ILogger<CestaRepositorio> logger)
    {
        _database = redis.GetDatabase();
        _logger = logger;
    }

    public async Task<CestaCliente?> ObterCestaAsync(string idCliente)
    {
        var dados = await _database.StringGetAsync(idCliente);
        if (dados.IsNullOrEmpty)
        {
            return null;
        }

        return JsonSerializer.Deserialize<CestaCliente>((string)dados!);
    }

    public async Task<CestaCliente?> AtualizarCestaAsync(CestaCliente cesta)
    {
        var criado = await _database.StringSetAsync(
            cesta.IdCliente,
            JsonSerializer.Serialize(cesta),
            TimeSpan.FromDays(30));

        if (!criado)
        {
            _logger.LogWarning("Falha ao persistir cesta para cliente {IdCliente}", cesta.IdCliente);
            return null;
        }

        return await ObterCestaAsync(cesta.IdCliente);
    }

    public async Task<bool> RemoverCestaAsync(string idCliente)
    {
        return await _database.KeyDeleteAsync(idCliente);
    }
}
