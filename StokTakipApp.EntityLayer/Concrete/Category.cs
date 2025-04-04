using System.Collections.Generic;

namespace StokTakipApp.EntityLayer.Concrete
{
    public class Category
    {
        public int Id { get; set; } // Birincil anahtar
        public string Name { get; set; } // Kategori adı
        public string Description { get; set; } // Açıklama
        public bool IsActive { get; set; } = true; // Aktiflik durumu (Tabloda en son sütun olarak)                                                   
        public virtual ICollection<Brand> Brands { get; set; } // Category'in birçok Brand ile ilişkisi var
    }
}