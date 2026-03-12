namespace VendeTudo.Pedidos.Dominio;

/// <summary>
/// Entidade representando um item dentro de um pedido.
/// </summary>
public class ItemPedido : Entidade
{
    /// <summary>
    /// Identificador do produto.
    /// </summary>
    public int IdProduto { get; private set; }

    /// <summary>
    /// Nome do produto.
    /// </summary>
    public string NomeProduto { get; private set; } = string.Empty;

    /// <summary>
    /// URL da imagem do produto.
    /// </summary>
    public string UrlImagem { get; private set; } = string.Empty;

    /// <summary>
    /// Preço unitário do produto.
    /// </summary>
    public decimal PrecoUnitario { get; private set; }

    /// <summary>
    /// Quantidade de unidades.
    /// </summary>
    public int Unidades { get; private set; }

    /// <summary>
    /// Desconto aplicado ao item.
    /// </summary>
    public decimal Desconto { get; private set; }

    protected ItemPedido()
    {
    }

    /// <summary>
    /// Cria um novo item de pedido.
    /// </summary>
    public ItemPedido(int idProduto, string nomeProduto, decimal precoUnitario, string urlImagem, int unidades = 1, decimal desconto = 0)
    {
        if (unidades <= 0)
        {
            throw new ExcecaoDominio("Quantidade de unidades inválida");
        }

        if (precoUnitario < 0)
        {
            throw new ExcecaoDominio("Preço unitário não pode ser negativo");
        }

        Id = Guid.NewGuid();
        IdProduto = idProduto;
        NomeProduto = nomeProduto;
        PrecoUnitario = precoUnitario;
        UrlImagem = urlImagem;
        Unidades = unidades;
        Desconto = desconto;
    }

    /// <summary>
    /// Obtém o preço unitário do item.
    /// </summary>
    public decimal GetPrecoUnitario()
    {
        return PrecoUnitario;
    }

    /// <summary>
    /// Obtém a quantidade de unidades.
    /// </summary>
    public int GetUnidades()
    {
        return Unidades;
    }

    /// <summary>
    /// Define novas unidades para o item.
    /// </summary>
    public void DefinirUnidades(int unidades)
    {
        if (unidades <= 0)
        {
            throw new ExcecaoDominio("Quantidade de unidades inválida");
        }

        Unidades = unidades;
    }

    /// <summary>
    /// Adiciona unidades ao item.
    /// </summary>
    public void AdicionarUnidades(int unidades)
    {
        if (unidades <= 0)
        {
            throw new ExcecaoDominio("Quantidade de unidades inválida");
        }

        Unidades += unidades;
    }
}
