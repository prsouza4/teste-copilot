using Microsoft.EntityFrameworkCore;

namespace VendeTudo.Catalogo.API;

/// <summary>
/// Contexto do banco de dados para o catálogo.
/// </summary>
public class CatalogoDbContext : DbContext
{
    public DbSet<ItemCatalogo> Itens { get; set; } = null!;
    public DbSet<TipoProduto> Tipos { get; set; } = null!;
    public DbSet<MarcaProduto> Marcas { get; set; } = null!;

    public CatalogoDbContext(DbContextOptions<CatalogoDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TipoProduto>(builder =>
        {
            builder.ToTable("TiposProduto");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Tipo).IsRequired().HasMaxLength(128);
        });

        modelBuilder.Entity<MarcaProduto>(builder =>
        {
            builder.ToTable("MarcasProduto");
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Marca).IsRequired().HasMaxLength(128);
        });

        modelBuilder.Entity<ItemCatalogo>(builder =>
        {
            builder.ToTable("ItensCatalogo");
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Nome).IsRequired().HasMaxLength(256);
            builder.Property(i => i.Descricao).HasMaxLength(1000);
            builder.Property(i => i.Preco).IsRequired().HasPrecision(18, 2);
            builder.Property(i => i.UrlImagem).HasMaxLength(512);
            builder.Property(i => i.NomeArquivoImagem).HasMaxLength(256);
            builder.Property(i => i.QuantidadeEstoque).IsRequired();
            builder.Property(i => i.UnidadeLimiteCompra).IsRequired();
            builder.Property(i => i.Disponivel).IsRequired();

            builder.HasOne(i => i.TipoProduto)
                .WithMany()
                .HasForeignKey(i => i.IdTipoProduto);

            builder.HasOne(i => i.MarcaProduto)
                .WithMany()
                .HasForeignKey(i => i.IdMarcaProduto);
        });
    }
}
