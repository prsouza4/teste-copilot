namespace VendeTudo.Compartilhado;

/// <summary>
/// Evento publicado quando o preço de um item do catálogo é alterado.
/// </summary>
public class PrecoItemAlteradoEventoIntegracao : EventoIntegracaoBase
{
    /// <summary>
    /// Identificador do produto.
    /// </summary>
    public int IdProduto { get; }

    /// <summary>
    /// Preço anterior do produto.
    /// </summary>
    public decimal PrecoAntigo { get; }

    /// <summary>
    /// Novo preço do produto.
    /// </summary>
    public decimal PrecoNovo { get; }

    public PrecoItemAlteradoEventoIntegracao(int idProduto, decimal precoAntigo, decimal precoNovo)
    {
        IdProduto = idProduto;
        PrecoAntigo = precoAntigo;
        PrecoNovo = precoNovo;
    }
}
