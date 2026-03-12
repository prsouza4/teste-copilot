namespace VendeTudo.Pedidos.Dominio;

/// <summary>
/// Classe base para entidades do domínio.
/// </summary>
public abstract class Entidade
{
    private int? _requestedHashCode;
    private Guid _id;

    /// <summary>
    /// Identificador da entidade.
    /// </summary>
    public virtual Guid Id
    {
        get => _id;
        protected set => _id = value;
    }

    private List<EventoDominio> _eventosDominio = new();

    /// <summary>
    /// Eventos de domínio pendentes.
    /// </summary>
    public IReadOnlyCollection<EventoDominio> EventosDominio => _eventosDominio.AsReadOnly();

    /// <summary>
    /// Adiciona um evento de domínio.
    /// </summary>
    public void AdicionarEventoDominio(EventoDominio evento)
    {
        _eventosDominio.Add(evento);
    }

    /// <summary>
    /// Remove um evento de domínio.
    /// </summary>
    public void RemoverEventoDominio(EventoDominio evento)
    {
        _eventosDominio.Remove(evento);
    }

    /// <summary>
    /// Limpa todos os eventos de domínio.
    /// </summary>
    public void LimparEventosDominio()
    {
        _eventosDominio.Clear();
    }

    public bool EhTransiente()
    {
        return Id == default;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entidade other)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (GetType() != other.GetType())
        {
            return false;
        }

        if (EhTransiente() || other.EhTransiente())
        {
            return false;
        }

        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        if (!EhTransiente())
        {
            _requestedHashCode ??= Id.GetHashCode() ^ 31;
            return _requestedHashCode.Value;
        }

        return base.GetHashCode();
    }

    public static bool operator ==(Entidade? left, Entidade? right)
    {
        return Equals(left, null) ? Equals(right, null) : left.Equals(right);
    }

    public static bool operator !=(Entidade? left, Entidade? right)
    {
        return !(left == right);
    }
}
