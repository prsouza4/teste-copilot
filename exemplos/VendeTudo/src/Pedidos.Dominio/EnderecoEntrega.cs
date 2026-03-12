namespace VendeTudo.Pedidos.Dominio;

/// <summary>
/// Value object representando o endereço de entrega.
/// </summary>
public record EnderecoEntrega(string Rua, string Cidade, string Estado, string Pais, string Cep)
{
    /// <summary>
    /// Cria um novo endereço de entrega validado.
    /// </summary>
    public static EnderecoEntrega Criar(string rua, string cidade, string estado, string pais, string cep)
    {
        if (string.IsNullOrWhiteSpace(rua))
        {
            throw new ExcecaoDominio("Rua é obrigatória");
        }

        if (string.IsNullOrWhiteSpace(cidade))
        {
            throw new ExcecaoDominio("Cidade é obrigatória");
        }

        if (string.IsNullOrWhiteSpace(estado))
        {
            throw new ExcecaoDominio("Estado é obrigatório");
        }

        if (string.IsNullOrWhiteSpace(pais))
        {
            throw new ExcecaoDominio("País é obrigatório");
        }

        if (string.IsNullOrWhiteSpace(cep))
        {
            throw new ExcecaoDominio("CEP é obrigatório");
        }

        return new EnderecoEntrega(rua, cidade, estado, pais, cep);
    }
}
