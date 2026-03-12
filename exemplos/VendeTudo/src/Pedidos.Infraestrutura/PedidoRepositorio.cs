using Microsoft.EntityFrameworkCore;
using VendeTudo.Pedidos.Dominio;

namespace VendeTudo.Pedidos.Infraestrutura;

/// <summary>
/// Implementação do repositório de pedidos usando Entity Framework Core.
/// </summary>
public class PedidoRepositorio : IPedidoRepositorio
{
    private readonly PedidosDbContext _context;

    public PedidoRepositorio(PedidosDbContext context)
    {
        _context = context;
    }

    public async Task<PedidoAgregado?> ObterPorIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Pedidos
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<IEnumerable<PedidoAgregado>> ObterPorCompradorAsync(string idComprador, CancellationToken ct = default)
    {
        return await _context.Pedidos
            .Where(p => p.IdComprador == idComprador)
            .OrderByDescending(p => p.DataCriacao)
            .ToListAsync(ct);
    }

    public async Task AdicionarAsync(PedidoAgregado pedido, CancellationToken ct = default)
    {
        await _context.Pedidos.AddAsync(pedido, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task AtualizarAsync(PedidoAgregado pedido, CancellationToken ct = default)
    {
        _context.Pedidos.Update(pedido);
        await _context.SaveChangesAsync(ct);
    }

    public async Task RemoverAsync(PedidoAgregado pedido, CancellationToken ct = default)
    {
        _context.Pedidos.Remove(pedido);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<PedidoAgregado>> ObterTodosAsync(CancellationToken ct = default)
    {
        return await _context.Pedidos
            .OrderByDescending(p => p.DataCriacao)
            .ToListAsync(ct);
    }
}
