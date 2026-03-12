namespace VendeTudo.Compartilhado;

/// <summary>
/// Item incluído no checkout.
/// </summary>
public class ItemCheckout
{
    /// <summary>
    /// Identificador do produto.
    /// </summary>
    public int IdProduto { get; set; }

    /// <summary>
    /// Nome do produto.
    /// </summary>
    public string NomeProduto { get; set; } = string.Empty;

    /// <summary>
    /// Preço unitário do produto.
    /// </summary>
    public decimal PrecoUnitario { get; set; }

    /// <summary>
    /// Quantidade do produto.
    /// </summary>
    public int Quantidade { get; set; }

    /// <summary>
    /// URL da imagem do produto.
    /// </summary>
    public string UrlImagem { get; set; } = string.Empty;
}
