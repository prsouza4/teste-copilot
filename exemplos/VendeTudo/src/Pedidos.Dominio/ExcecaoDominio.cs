namespace VendeTudo.Pedidos.Dominio;

/// <summary>
/// Exceção para erros de domínio.
/// </summary>
public class ExcecaoDominio : Exception
{
    public ExcecaoDominio()
    {
    }

    public ExcecaoDominio(string mensagem) : base(mensagem)
    {
    }

    public ExcecaoDominio(string mensagem, Exception innerException) : base(mensagem, innerException)
    {
    }
}
