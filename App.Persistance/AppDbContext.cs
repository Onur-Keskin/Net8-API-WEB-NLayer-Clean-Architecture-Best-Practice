using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace App.Persistance
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<Category> Categories { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Product>()
                .ToTable("Products", tb => tb.HasTrigger("trg_Products_AllActions"));

            modelBuilder.Entity<Category>()
                .ToTable("Categories", tb => tb.HasTrigger("trg_Categories_AllActions"));

            modelBuilder.Entity<User>()
                .ToTable("Users", tb => tb.HasTrigger("trg_Users_AllActions"));

            base.OnModelCreating(modelBuilder);
        }
    }
}
