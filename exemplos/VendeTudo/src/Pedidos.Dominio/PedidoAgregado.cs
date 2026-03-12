namespace VendeTudo.Pedidos.Dominio;

/// <summary>
/// Agregado representando um pedido.
/// </summary>
public class PedidoAgregado : Entidade, IRaizAgregado
{
    /// <summary>
    /// Endereço de entrega do pedido.
    /// </summary>
    public EnderecoEntrega Endereco { get; private set; } = null!;

    /// <summary>
    /// Status atual do pedido.
    /// </summary>
    public StatusPedido Status { get; private set; }

    /// <summary>
    /// Identificador do comprador.
    /// </summary>
    public string IdComprador { get; private set; } = string.Empty;

    /// <summary>
    /// Data e hora de criação do pedido.
    /// </summary>
    public DateTime DataCriacao { get; private set; }

    /// <summary>
    /// Descrição do pedido.
    /// </summary>
    public string? Descricao { get; private set; }

    private readonly List<ItemPedido> _itens = new();

    /// <summary>
    /// Itens do pedido.
    /// </summary>
    public IReadOnlyCollection<ItemPedido> Itens => _itens.AsReadOnly();

    /// <summary>
    /// Total do pedido.
    /// </summary>
    public decimal Total => _itens.Sum(i => i.GetPrecoUnitario() * i.GetUnidades());

    protected PedidoAgregado()
    {
    }

    /// <summary>
    /// Cria um novo pedido.
    /// </summary>
    public static PedidoAgregado CriarPedido(string idComprador, EnderecoEntrega endereco)
    {
        if (string.IsNullOrWhiteSpace(idComprador))
        {
            throw new ExcecaoDominio("Identificador do comprador é obrigatório");
        }

        if (endereco is null)
        {
            throw new ExcecaoDominio("Endereço de entrega é obrigatório");
        }

        var pedido = new PedidoAgregado
        {
            Id = Guid.NewGuid(),
            IdComprador = idComprador,
            Endereco = endereco,
            Status = StatusPedido.Submetido,
            DataCriacao = DateTime.UtcNow
        };

        pedido.AdicionarEventoDominio(new PedidoCriadoEventoDominio(pedido.Id, idComprador));

        return pedido;
    }

    /// <summary>
    /// Adiciona um item ao pedido.
    /// </summary>
    public void AdicionarItemPedido(int idProduto, string nome, decimal preco, string urlImagem, int quantidade = 1)
    {
        if (Status != StatusPedido.Submetido)
        {
            throw new ExcecaoDominio("Não é possível adicionar itens a um pedido que não está com status Submetido");
        }

        var itemExistente = _itens.FirstOrDefault(i => i.IdProduto == idProduto);

        if (itemExistente is not null)
        {
            itemExistente.AdicionarUnidades(quantidade);
        }
        else
        {
            var item = new ItemPedido(idProduto, nome, preco, urlImagem, quantidade);
            _itens.Add(item);
        }
    }

    /// <summary>
    /// Define o status como pago pendente.
    /// </summary>
    public void DefinirStatusPagoPendente()
    {
        if (Status != StatusPedido.EstoqueConfirmado)
        {
            throw new ExcecaoDominio("Não é possível definir como pago pendente sem confirmação de estoque");
        }

        Status = StatusPedido.Pago;
        AdicionarEventoDominio(new PedidoPagoEventoDominio(Id));
    }

    /// <summary>
    /// Define o status como aguardando validação.
    /// </summary>
    public void DefinirStatusAguardandoValidacao()
    {
        if (Status != StatusPedido.Submetido)
        {
            throw new ExcecaoDominio("Não é possível definir como aguardando validação a partir do status atual");
        }

        Status = StatusPedido.AguardandoValidacao;
    }

    /// <summary>
    /// Define o status como estoque confirmado.
    /// </summary>
    public void DefinirStatusEstoqueConfirmado()
    {
        if (Status != StatusPedido.AguardandoValidacao)
        {
            throw new ExcecaoDominio("Não é possível confirmar estoque a partir do status atual");
        }

        Status = StatusPedido.EstoqueConfirmado;
    }

    /// <summary>
    /// Define o status como enviado.
    /// </summary>
    public void DefinirStatusEnviado()
    {
        if (Status != StatusPedido.Pago)
        {
            throw new ExcecaoDominio("Não é possível enviar um pedido que não foi pago");
        }

        Status = StatusPedido.Enviado;
        AdicionarEventoDominio(new PedidoEnviadoEventoDominio(Id));
    }

    /// <summary>
    /// Cancela o pedido.
    /// </summary>
    public void Cancelar()
    {
        if (Status == StatusPedido.Enviado)
        {
            throw new ExcecaoDominio("Não é possível cancelar um pedido já enviado");
        }

        if (Status == StatusPedido.Cancelado)
        {
            throw new ExcecaoDominio("Pedido já está cancelado");
        }

        Status = StatusPedido.Cancelado;
        AdicionarEventoDominio(new PedidoCanceladoEventoDominio(Id));
    }

    /// <summary>
    /// Define a descrição do pedido.
    /// </summary>
    public void DefinirDescricao(string descricao)
    {
        Descricao = descricao;
    }
}
