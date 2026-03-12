using Microsoft.EntityFrameworkCore;
using VendeTudo.Pedidos.Dominio;

namespace VendeTudo.Pedidos.Infraestrutura;

/// <summary>
/// Contexto do banco de dados para pedidos.
/// </summary>
public class PedidosDbContext : DbContext
{
    public DbSet<PedidoAgregado> Pedidos { get; set; } = null!;
    public DbSet<ItemPedido> ItensPedido { get; set; } = null!;

    public PedidosDbContext(DbContextOptions<PedidosDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PedidoAgregado>(builder =>
        {
            builder.ToTable("Pedidos");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.IdComprador).IsRequired().HasMaxLength(256);
            builder.Property(p => p.Status).IsRequired();
            builder.Property(p => p.DataCriacao).IsRequired();
            builder.Property(p => p.Descricao).HasMaxLength(500);

            builder.OwnsOne(p => p.Endereco, endereco =>
            {
                endereco.Property(e => e.Rua).HasColumnName("EnderecoRua").HasMaxLength(256);
                endereco.Property(e => e.Cidade).HasColumnName("EnderecoCidade").HasMaxLength(128);
                endereco.Property(e => e.Estado).HasColumnName("EnderecoEstado").HasMaxLength(128);
                endereco.Property(e => e.Pais).HasColumnName("EnderecoPais").HasMaxLength(128);
                endereco.Property(e => e.Cep).HasColumnName("EnderecoCep").HasMaxLength(20);
            });

            builder.HasMany(p => p.Itens)
                .WithOne()
                .HasForeignKey("PedidoId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(p => p.Itens).AutoInclude();

            builder.Ignore(p => p.EventosDominio);
        });

        modelBuilder.Entity<ItemPedido>(builder =>
        {
            builder.ToTable("ItensPedido");
            builder.HasKey(i => i.Id);
            builder.Property(i => i.IdProduto).IsRequired();
            builder.Property(i => i.NomeProduto).IsRequired().HasMaxLength(256);
            builder.Property(i => i.UrlImagem).HasMaxLength(512);
            builder.Property(i => i.PrecoUnitario).IsRequired().HasPrecision(18, 2);
            builder.Property(i => i.Unidades).IsRequired();
            builder.Property(i => i.Desconto).HasPrecision(18, 2);

            builder.Ignore(i => i.EventosDominio);
        });
    }
}
