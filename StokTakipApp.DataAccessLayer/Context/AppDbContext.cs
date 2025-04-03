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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Category tablosu için Fluent API yapılandırması
            modelBuilder.Entity<Category>()
                .ToTable("Categories") // Veritabanındaki tablo adını belirler
                .HasKey(c => c.Id); // Birincil anahtar tanımlaması
                
            modelBuilder.Entity<Category>()
                .Property(c => c.Name)                
                .HasMaxLength(100);

            modelBuilder.Entity<Category>()
                .Property(c => c.Description)
                .HasMaxLength(500);

            modelBuilder.Entity<Category>()
                .Property(c => c.IsActive)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}