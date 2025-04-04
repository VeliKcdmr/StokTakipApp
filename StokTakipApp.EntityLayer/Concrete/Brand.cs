
using System.Collections.Generic;

namespace StokTakipApp.EntityLayer.Concrete
{
    public class Brand
    {
        public int Id { get; set; } // Birincil anahtar
        public string Name { get; set; } // Marka adı
        public string Description { get; set; } // Açıklama
        public bool IsActive { get; set; } = true; // Aktiflik durumu
        public int CategoryId { get; set; } // Kategori ile ilişki (Tabloda en son)
        public virtual Category Category { get; set; } // Kategori tablosuyla navigasyon
        public virtual ICollection<Model> Models { get; set; } // Brand'in birçok Model ile ilişkisi var

    }
}
