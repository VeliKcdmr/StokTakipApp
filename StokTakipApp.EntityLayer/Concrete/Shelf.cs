using StokTakipApp.EntityLayer.Concrete;
using System.Collections.Generic;

public class Shelf
{
    public int Id { get; set; } // Birincil anahtar
    public string ShelfName { get; set; } // Raf adı    
    public bool IsActive { get; set; } = true; // Rafın aktif olup olmadığı
    public virtual ICollection<Product> Products { get; set; } // Raf'taki ürünler
}