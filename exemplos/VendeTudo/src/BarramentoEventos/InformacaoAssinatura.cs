namespace VendeTudo.BarramentoEventos;

/// <summary>
/// Informações sobre uma assinatura de evento.
/// </summary>
public class InformacaoAssinatura
{
    /// <summary>
    /// Tipo do manipulador do evento.
    /// </summary>
    public Type TipoManipulador { get; }

    public InformacaoAssinatura(Type tipoManipulador)
    {
        TipoManipulador = tipoManipulador;
    }
}
