namespace VendeTudo.Pedidos.Dominio;

/// <summary>
/// Evento de domínio disparado quando um pedido é criado.
/// </summary>
public class PedidoCriadoEventoDominio : EventoDominio
{
    public Guid IdPedido { get; }
    public string IdComprador { get; }

    public PedidoCriadoEventoDominio(Guid idPedido, string idComprador)
    {
        IdPedido = idPedido;
        IdComprador = idComprador;
    }
}

/// <summary>
/// Evento de domínio disparado quando um pedido é pago.
/// </summary>
public class PedidoPagoEventoDominio : EventoDominio
{
    public Guid IdPedido { get; }

    public PedidoPagoEventoDominio(Guid idPedido)
    {
        IdPedido = idPedido;
    }
}

/// <summary>
/// Evento de domínio disparado quando um pedido é enviado.
/// </summary>
public class PedidoEnviadoEventoDominio : EventoDominio
{
    public Guid IdPedido { get; }

    public PedidoEnviadoEventoDominio(Guid idPedido)
    {
        IdPedido = idPedido;
    }
}

/// <summary>
/// Evento de domínio disparado quando um pedido é cancelado.
/// </summary>
public class PedidoCanceladoEventoDominio : EventoDominio
{
    public Guid IdPedido { get; }

    public PedidoCanceladoEventoDominio(Guid idPedido)
    {
        IdPedido = idPedido;
    }
}
