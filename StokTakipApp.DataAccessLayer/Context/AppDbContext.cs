using System.Data.Entity;
using StokTakipApp.EntityLayer.Concrete;

namespace StokTakipApp.DataAccessLayer.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("AppDbContext") // Bağlantı dizesinin adını burada veriyoruz
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Category yapılandırması
            modelBuilder.Entity<Category>()
                .ToTable("Categories")
                .HasKey(c => c.Id);

            modelBuilder.Entity<Category>()
                .Property(c => c.Name)
                .HasMaxLength(100);

            modelBuilder.Entity<Category>()
                .Property(c => c.Description)
                .HasMaxLength(500);

            modelBuilder.Entity<Category>()
                .Property(c => c.IsActive)
                .IsRequired();

            // Brand yapılandırması
            modelBuilder.Entity<Brand>()
                .ToTable("Brands")
                .HasKey(b => b.Id);

            modelBuilder.Entity<Brand>()
                .Property(b => b.Name)
                .HasMaxLength(100);

            modelBuilder.Entity<Brand>()
                .Property(b => b.Description)
                .HasMaxLength(500);

            modelBuilder.Entity<Brand>()
                .Property(b => b.IsActive)
                .IsRequired();

            modelBuilder.Entity<Brand>()
                .HasRequired(b => b.Category)
                .WithMany(c => c.Brands)
                .HasForeignKey(b => b.CategoryId);

            // Model yapılandırması
            modelBuilder.Entity<Model>()
                .ToTable("Models")
                .HasKey(m => m.Id);

            modelBuilder.Entity<Model>()
                .Property(m => m.Name)
                .HasMaxLength(100);

            modelBuilder.Entity<Model>()
                .Property(m => m.Description)
                .HasMaxLength(500);

            modelBuilder.Entity<Model>()
                .Property(m => m.Year)
                .IsRequired();

            modelBuilder.Entity<Model>()
                .Property(m => m.IsActive)
                .IsRequired();

            modelBuilder.Entity<Model>()
                .HasRequired(m => m.Brand)
                .WithMany(b => b.Models)
                .HasForeignKey(m => m.BrandId);

            // Product yapılandırması
            modelBuilder.Entity<Product>()
                .ToTable("Products")
                .HasKey(p => p.Id);

            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .HasMaxLength(100);

            modelBuilder.Entity<Product>()
                .Property(p => p.Barcode)
                .HasMaxLength(50);

            modelBuilder.Entity<Product>()
                .Property(p => p.Description)
                .HasMaxLength(500);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .Property(p => p.IsActive)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .HasRequired(p => p.Model)
                .WithMany(m => m.Products)
                .HasForeignKey(p => p.ModelId);


            base.OnModelCreating(modelBuilder);
        }
    }
}