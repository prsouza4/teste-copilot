using ApiEnderecos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiEnderecos.Infrastructure.Persistence;

public class AddressDbContext : DbContext
{
    public DbSet<Address> Addresses { get; set; }

    public AddressDbContext(DbContextOptions<AddressDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Street).IsRequired().HasMaxLength(255);
            entity.Property(a => a.City).IsRequired().HasMaxLength(100);
            entity.Property(a => a.State).IsRequired().HasMaxLength(50);
            entity.Property(a => a.ZipCode).IsRequired().HasMaxLength(20);
            entity.HasIndex(a => new { a.Street, a.City, a.State, a.ZipCode }).IsUnique();
        });
    }
}
