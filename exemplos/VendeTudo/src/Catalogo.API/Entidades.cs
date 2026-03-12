namespace VendeTudo.Catalogo.API;

/// <summary>
/// Entidade representando um tipo de produto.
/// </summary>
public class TipoProduto
{
    public int Id { get; set; }
    public string Tipo { get; set; } = string.Empty;
}

/// <summary>
/// Entidade representando uma marca de produto.
/// </summary>
public class MarcaProduto
{
    public int Id { get; set; }
    public string Marca { get; set; } = string.Empty;
}

/// <summary>
/// Entidade representando um item do catálogo.
/// </summary>
public class ItemCatalogo
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public string? UrlImagem { get; set; }
    public string? NomeArquivoImagem { get; set; }
    public int QuantidadeEstoque { get; set; }
    public int UnidadeLimiteCompra { get; set; } = 10;
    public bool Disponivel { get; set; } = true;
    
    public int IdTipoProduto { get; set; }
    public TipoProduto? TipoProduto { get; set; }
    
    public int IdMarcaProduto { get; set; }
    public MarcaProduto? MarcaProduto { get; set; }
}
