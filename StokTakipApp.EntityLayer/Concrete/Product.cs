﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakipApp.EntityLayer.Concrete
{
    public class Product
    {
        public int Id { get; set; } // Birincil anahtar
        public string Name { get; set; } // Ürün adı
        public string Barcode { get; set; } // Barkod
        public int ModelId { get; set; } // Model ile ilişki (Tabloda en son)
        public int ShelfId { get; set; } // Raf ile ilişki için Foreign Key
        public decimal Price { get; set; } // Ürün fiyatı
        public int StockQuantity { get; set; } // Stok miktarı
        public string Description { get; set; } // Açıklama
        public bool IsActive { get; set; } = true; // Aktiflik durumu
        
        public virtual Model Model { get; set; } // Model tablosuyla navigasyon
        public virtual Shelf Shelf { get; set; } // Navigasyon özelliği

    }
}
