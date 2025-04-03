namespace StokTakipApp.EntityLayer.Concrete
{
    public class Category
    {
        public int Id { get; set; } // Benzersiz kategori tanımlayıcısı
        public string Name { get; set; } // Kategori adı
        public string Description { get; set; } // Kategori açıklaması
        public bool IsActive { get; set; } // Kategorinin aktiflik durumu

        // Varsayılan constructor
        public Category()
        {
            IsActive = true; // Varsayılan olarak aktif
        }
    }
}