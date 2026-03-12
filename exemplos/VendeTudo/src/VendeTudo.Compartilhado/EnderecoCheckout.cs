namespace VendeTudo.Compartilhado;

/// <summary>
/// Endereço de entrega para o checkout.
/// </summary>
public class EnderecoCheckout
{
    /// <summary>
    /// Rua do endereço.
    /// </summary>
    public string Rua { get; set; } = string.Empty;

    /// <summary>
    /// Cidade do endereço.
    /// </summary>
    public string Cidade { get; set; } = string.Empty;

    /// <summary>
    /// Estado do endereço.
    /// </summary>
    public string Estado { get; set; } = string.Empty;

    /// <summary>
    /// País do endereço.
    /// </summary>
    public string Pais { get; set; } = string.Empty;

    /// <summary>
    /// CEP do endereço.
    /// </summary>
    public string Cep { get; set; } = string.Empty;
}
