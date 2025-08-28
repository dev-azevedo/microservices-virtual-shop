using Microsoft.EntityFrameworkCore;
using VShop.ProductApi.Models;

namespace VShop.ProductApi.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }


    // Fluent API   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Category
        modelBuilder.Entity<Category>().HasKey(c => c.CategoryId);
        modelBuilder.Entity<Category>().Property(c => c.Name).IsRequired().HasMaxLength(100);
        modelBuilder.Entity<Category>().HasMany(c => c.Products).WithOne(p => p.Category).IsRequired()
            .HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.Cascade);
        
        // Product
        modelBuilder.Entity<Product>().HasKey(p => p.Id);
        modelBuilder.Entity<Product>().Property(p => p.Name).IsRequired().HasMaxLength(100);
        modelBuilder.Entity<Product>().Property(p => p.Price).IsRequired().HasPrecision(12, 2);
        modelBuilder.Entity<Product>().Property(p => p.Description).IsRequired().HasMaxLength(255);
        modelBuilder.Entity<Product>().Property(p => p.Stock).IsRequired();
        modelBuilder.Entity<Product>().Property(p => p.ImageUrl).IsRequired().HasMaxLength(255);
        modelBuilder.Entity<Product>().HasOne(p => p.Category).WithMany(c => c.Products);



        modelBuilder.Entity<Category>().HasData(
            new Category { CategoryId = 1, Name = "Material Escolar" },
            new Category { CategoryId = 2, Name = "Acessórios" }
        );
    }
}
