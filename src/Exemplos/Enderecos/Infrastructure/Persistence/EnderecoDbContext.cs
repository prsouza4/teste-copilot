using Exemplos.Enderecos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Exemplos.Enderecos.Infrastructure.Persistence;

public class EnderecoDbContext : DbContext
{
    public DbSet<Endereco> Enderecos { get; set; }

    public EnderecoDbContext(DbContextOptions<EnderecoDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Endereco>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Rua).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Numero).IsRequired().HasMaxLength(10);
            entity.Property(e => e.Bairro).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Cidade).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Estado).IsRequired().HasMaxLength(2);
            entity.Property(e => e.Cep).IsRequired().HasMaxLength(8);
            entity.HasIndex(e => new { e.Rua, e.Numero, e.Bairro, e.Cidade, e.Estado, e.Cep }).IsUnique();
        });
    }
}
