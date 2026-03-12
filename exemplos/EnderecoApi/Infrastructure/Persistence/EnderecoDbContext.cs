using EnderecoApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnderecoApi.Infrastructure.Persistence;

public class EnderecoDbContext : DbContext
{
    public EnderecoDbContext(DbContextOptions<EnderecoDbContext> options) : base(options) { }

    public DbSet<Endereco> Enderecos => Set<Endereco>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Endereco>()
            .HasIndex(e => new { e.Rua, e.Numero, e.Cidade, e.Estado, e.Cep })
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}
