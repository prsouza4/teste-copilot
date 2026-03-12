using VendeTudo.Compartilhado;

namespace VendeTudo.Cesta.API;

/// <summary>
/// Manipulador do evento de preço alterado.
/// </summary>
public class PrecoItemAlteradoManipulador : IManipuladorEventoIntegracao<PrecoItemAlteradoEventoIntegracao>
{
    private readonly ICestaRepositorio _repositorio;
    private readonly ILogger<PrecoItemAlteradoManipulador> _logger;

    public PrecoItemAlteradoManipulador(
        ICestaRepositorio repositorio,
        ILogger<PrecoItemAlteradoManipulador> logger)
    {
        _repositorio = repositorio;
        _logger = logger;
    }

    public async Task ManipularAsync(PrecoItemAlteradoEventoIntegracao evento)
    {
        _logger.LogInformation(
            "Processando alteração de preço do produto {IdProduto}: {PrecoAntigo} -> {PrecoNovo}",
            evento.IdProduto, evento.PrecoAntigo, evento.PrecoNovo);

        // Em uma implementação real, seria necessário iterar sobre todas as cestas
        // que contêm o produto e atualizar o preço.
        // Por simplicidade, este manipulador apenas registra o evento.
    }
}
