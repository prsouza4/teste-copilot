using API.Cadastro.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Cadastro.Data;

/// <summary>
/// Entity Framework Core DbContext for the Cadastro database.
/// </summary>
public class CadastroDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of CadastroDbContext.
    /// </summary>
    public CadastroDbContext(DbContextOptions<CadastroDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the Pessoas DbSet.
    /// </summary>
    public DbSet<Pessoa> Pessoas => Set<Pessoa>();

    /// <summary>
    /// Configures the model for the context.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Pessoa>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Telefone).HasMaxLength(50);
        });
    }

    /// <summary>
    /// Seeds the database with initial data.
    /// </summary>
    public static void SeedData(CadastroDbContext context)
    {
        if (!context.Pessoas.Any())
        {
            context.Pessoas.AddRange(
                new Pessoa
                {
                    Id = Guid.NewGuid(),
                    Nome = "Maria Silva",
                    Email = "maria.silva@exemplo.com",
                    Telefone = "(11) 99999-0001",
                    DataCadastro = DateTime.UtcNow.AddDays(-30)
                },
                new Pessoa
                {
                    Id = Guid.NewGuid(),
                    Nome = "João Santos",
                    Email = "joao.santos@exemplo.com",
                    Telefone = "(11) 99999-0002",
                    DataCadastro = DateTime.UtcNow.AddDays(-15)
                },
                new Pessoa
                {
                    Id = Guid.NewGuid(),
                    Nome = "Ana Oliveira",
                    Email = "ana.oliveira@exemplo.com",
                    Telefone = "(11) 99999-0003",
                    DataCadastro = DateTime.UtcNow.AddDays(-7)
                }
            );
            context.SaveChanges();
        }
    }
}
