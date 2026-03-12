using Endereco.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Endereco.Infrastructure.Persistence;

public class EnderecoDbContext : DbContext
{
    public EnderecoDbContext(DbContextOptions<EnderecoDbContext> options) : base(options) { }

    public DbSet<Endereco> Enderecos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Endereco>()
            .HasIndex(e => new { e.Rua, e.Numero, e.Cidade, e.Estado, e.CEP })
            .IsUnique();
    }
}
