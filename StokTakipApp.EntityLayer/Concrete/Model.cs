using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakipApp.EntityLayer.Concrete
{
    public class Model
    {
        public int Id { get; set; } // Birincil anahtar
        public string Name { get; set; } // Model adı       
        public int Year { get; set; } // Modelin yılı
        public string Description { get; set; } // Açıklama
        public int BrandId { get; set; } // Marka ile ilişki (yabancı anahtar)
        public bool IsActive { get; set; } = true; // Aktiflik durumu        
        public virtual Brand Brand { get; set; } // Marka tablosuyla navigasyon
        public virtual ICollection<Product> Products { get; set; }// Model'in birçok Product ile ilişkisi var

    }
}
