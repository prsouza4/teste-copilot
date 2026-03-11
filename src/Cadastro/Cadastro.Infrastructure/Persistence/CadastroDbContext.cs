using Cadastro.Domain.Entities;
using Cadastro.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Cadastro.Infrastructure.Persistence;

/// <summary>
/// Entity Framework Core database context for Cadastro.
/// </summary>
public class CadastroDbContext : DbContext
{
    public CadastroDbContext(DbContextOptions<CadastroDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");

            entity.HasKey(u => u.Id);

            entity.Property(u => u.Id)
                .ValueGeneratedNever();

            entity.Property(u => u.Name)
                .HasMaxLength(Name.MaxLength)
                .HasConversion(
                    n => n.Value,
                    v => Name.Create(v))
                .IsRequired();

            entity.Property(u => u.Email)
                .HasMaxLength(255)
                .HasConversion(
                    e => e.Value,
                    v => Email.Create(v))
                .IsRequired();

            entity.HasIndex(u => u.Email)
                .IsUnique();

            entity.Property(u => u.CreatedAt)
                .IsRequired();

            entity.Property(u => u.IsActive)
                .IsRequired();

            // Ignore domain events - they are not persisted
            entity.Ignore(u => u.DomainEvents);
        });
    }
}
