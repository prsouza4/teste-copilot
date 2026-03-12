using Microsoft.EntityFrameworkCore;
using Usuario.Domain.Entities;

namespace Usuario.Infrastructure.Persistence
{
    public class UserDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CPF).IsRequired().HasMaxLength(11);
                entity.Property(e => e.DateOfBirth).IsRequired();
                entity.Property(e => e.Profession).HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            });
        }
    }
}
