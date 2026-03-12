using VendeTudo.Compartilhado;

namespace VendeTudo.BarramentoEventos;

/// <summary>
/// Implementação em memória do gerenciador de assinaturas de eventos.
/// </summary>
public class GerenciadorAssinaturasEventosEmMemoria : IGerenciadorAssinaturasEventos
{
    private readonly Dictionary<string, List<InformacaoAssinatura>> _manipuladores = new();
    private readonly List<Type> _tiposEventos = new();

    public bool EstaVazio => _manipuladores.Count == 0;

    public event EventHandler<string>? AoRemoverEvento;

    public void AdicionarAssinatura<T, TM>()
        where T : EventoIntegracaoBase
        where TM : IManipuladorEventoIntegracao<T>
    {
        var nomeEvento = ObterNomeEvento<T>();

        if (!_manipuladores.ContainsKey(nomeEvento))
        {
            _manipuladores[nomeEvento] = new List<InformacaoAssinatura>();
        }

        var tipoManipulador = typeof(TM);
        if (_manipuladores[nomeEvento].Any(s => s.TipoManipulador == tipoManipulador))
        {
            throw new InvalidOperationException($"Manipulador {tipoManipulador.Name} já registrado para '{nomeEvento}'");
        }

        _manipuladores[nomeEvento].Add(new InformacaoAssinatura(tipoManipulador));

        if (!_tiposEventos.Contains(typeof(T)))
        {
            _tiposEventos.Add(typeof(T));
        }
    }

    public void RemoverAssinatura<T, TM>()
        where T : EventoIntegracaoBase
        where TM : IManipuladorEventoIntegracao<T>
    {
        var nomeEvento = ObterNomeEvento<T>();
        var tipoManipulador = typeof(TM);

        if (!_manipuladores.TryGetValue(nomeEvento, out var manipuladores))
        {
            return;
        }

        var assinatura = manipuladores.FirstOrDefault(s => s.TipoManipulador == tipoManipulador);
        if (assinatura is not null)
        {
            manipuladores.Remove(assinatura);

            if (manipuladores.Count == 0)
            {
                _manipuladores.Remove(nomeEvento);
                var tipoEvento = _tiposEventos.SingleOrDefault(e => e.Name == nomeEvento);
                if (tipoEvento is not null)
                {
                    _tiposEventos.Remove(tipoEvento);
                }

                RaiseAoRemoverEvento(nomeEvento);
            }
        }
    }

    public bool TemAssinaturasParaEvento<T>() where T : EventoIntegracaoBase
    {
        var nomeEvento = ObterNomeEvento<T>();
        return TemAssinaturasParaEvento(nomeEvento);
    }

    public bool TemAssinaturasParaEvento(string nomeEvento)
    {
        return _manipuladores.ContainsKey(nomeEvento);
    }

    public Type? ObterTipoEventoPorNome(string nomeEvento)
    {
        return _tiposEventos.SingleOrDefault(t => t.Name == nomeEvento);
    }

    public void Limpar()
    {
        _manipuladores.Clear();
        _tiposEventos.Clear();
    }

    public IEnumerable<InformacaoAssinatura> ObterManipuladoresParaEvento(string nomeEvento)
    {
        if (_manipuladores.TryGetValue(nomeEvento, out var manipuladores))
        {
            return manipuladores;
        }

        return Enumerable.Empty<InformacaoAssinatura>();
    }

    public string ObterNomeEvento<T>()
    {
        return typeof(T).Name;
    }

    private void RaiseAoRemoverEvento(string nomeEvento)
    {
        AoRemoverEvento?.Invoke(this, nomeEvento);
    }
}
