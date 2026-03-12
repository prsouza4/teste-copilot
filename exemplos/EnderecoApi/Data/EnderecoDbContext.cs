using EnderecoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EnderecoApi.Data;

public class EnderecoDbContext : DbContext
{
    public EnderecoDbContext(DbContextOptions<EnderecoDbContext> options) : base(options)
    {
    }

    public DbSet<Endereco> Enderecos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Endereco>()
            .HasIndex(e => new { e.Logradouro, e.Numero, e.Cidade, e.Estado, e.CEP })
            .IsUnique();
    }
}
