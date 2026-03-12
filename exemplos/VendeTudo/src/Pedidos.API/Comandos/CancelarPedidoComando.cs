using MediatR;
using VendeTudo.Pedidos.Dominio;

namespace VendeTudo.Pedidos.API.Comandos;

/// <summary>
/// Comando para cancelar um pedido.
/// </summary>
public record CancelarPedidoComando(Guid IdPedido) : IRequest<bool>;

/// <summary>
/// Handler para o comando de cancelar pedido.
/// </summary>
public class CancelarPedidoHandler : IRequestHandler<CancelarPedidoComando, bool>
{
    private readonly IPedidoRepositorio _repositorio;
    private readonly ILogger<CancelarPedidoHandler> _logger;

    public CancelarPedidoHandler(IPedidoRepositorio repositorio, ILogger<CancelarPedidoHandler> logger)
    {
        _repositorio = repositorio;
        _logger = logger;
    }

    public async Task<bool> Handle(CancelarPedidoComando request, CancellationToken cancellationToken)
    {
        var pedido = await _repositorio.ObterPorIdAsync(request.IdPedido, cancellationToken);
        if (pedido is null)
        {
            _logger.LogWarning("Pedido {IdPedido} não encontrado para cancelamento", request.IdPedido);
            return false;
        }

        try
        {
            pedido.Cancelar();
            await _repositorio.AtualizarAsync(pedido, cancellationToken);
            _logger.LogInformation("Pedido {IdPedido} cancelado com sucesso", request.IdPedido);
            return true;
        }
        catch (ExcecaoDominio ex)
        {
            _logger.LogWarning("Não foi possível cancelar pedido {IdPedido}: {Mensagem}",
                request.IdPedido, ex.Message);
            return false;
        }
    }
}
