namespace VendeTudo.Cesta.API;

/// <summary>
/// Representa um item na cesta de compras.
/// </summary>
public class ItemCesta
{
    public int IdProduto { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public decimal PrecoUnitario { get; set; }
    public decimal PrecoAntigo { get; set; }
    public int Quantidade { get; set; }
    public string? UrlImagem { get; set; }
}

/// <summary>
/// Representa a cesta de compras de um cliente.
/// </summary>
public class CestaCliente
{
    public string IdCliente { get; set; } = string.Empty;
    public List<ItemCesta> Itens { get; set; } = new();

    public decimal Total => Itens.Sum(i => i.PrecoUnitario * i.Quantidade);
}
