namespace VendeTudo.Compartilhado;

/// <summary>
/// Evento publicado quando o checkout é aceito e um pedido deve ser criado.
/// </summary>
public class CheckoutAceitoEventoIntegracao : EventoIntegracaoBase
{
    /// <summary>
    /// Identificador do comprador.
    /// </summary>
    public string IdComprador { get; }

    /// <summary>
    /// Itens do checkout.
    /// </summary>
    public List<ItemCheckout> Itens { get; }

    /// <summary>
    /// Endereço de entrega.
    /// </summary>
    public EnderecoCheckout Endereco { get; }

    public CheckoutAceitoEventoIntegracao(string idComprador, List<ItemCheckout> itens, EnderecoCheckout endereco)
    {
        IdComprador = idComprador;
        Itens = itens;
        Endereco = endereco;
    }
}
