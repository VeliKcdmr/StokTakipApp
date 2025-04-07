
using StokTakipApp.BusinessLayer.Concrete;
using StokTakipApp.DataAccessLayer.Context;
using StokTakipApp.DataAccessLayer.EntityFramework;
using System;
using System.Linq;
using System.Windows.Forms;

namespace StokTakipApp.PresentationLayer.Modules.Products
{
    
    public partial class FrmUrunler : DevExpress.XtraEditors.XtraForm
    {
        ProductManager _productManager = new ProductManager(new EfProductDal(new AppDbContext()));
        CategoryManager _categoryManager = new CategoryManager(new EfCategoryDal(new AppDbContext()));
        BrandManager _brandManager = new BrandManager(new EfBrandDal(new AppDbContext()));
        ModelManager _modelManager = new ModelManager(new EfModelDal(new AppDbContext()));
        ShelfManager _shelfManager = new ShelfManager(new EfShelfDal(new AppDbContext()));
        public int _productId;
        public int _categoryId;
        public int _brandId;        
        public int _modelId;
        public int _shelfId;

        public FrmUrunler()
        {
            InitializeComponent();
        }
        private void LoadProducts()
        {
            try
            {
                var categories = _categoryManager.TGetAll().ToList();
                var brands = _brandManager.TGetAll().ToList();
                var models = _modelManager.TGetAll().ToList();
                gridControl1.DataSource = null; // İlk başta boş bir yapı
                gridControl1.DataSource = _productManager.TGetAll().Select(p => new
                {
                    p.Id,
                    p.Name, // Ürün adı
                    p.Barcode, // Barkod
                    p.StockQuantity, // Stok miktarı
                    p.Price, // Ürün fiyatı
                    Category = categories.FirstOrDefault(c => c.Id == p.Model.Brand.CategoryId).Name,
                    Brand = brands.FirstOrDefault(b=>b.Id == p.Model.BrandId).Name,
                    ModelName = models.FirstOrDefault(m=>m.Id == p.Model.Id).Name,
                    ModelYear = models.FirstOrDefault(y=>y.Id==p.Model.Year), // Model yılı
                    ShelfName = p.Shelf.ShelfName, // Raf adı
                    IsActive = p.IsActive ? "Aktif" : "Pasif" // Durum için string değer
                }).ToList(); // Verileri listele

                // Sütun başlıklarını ayarla
                gridView1.Columns["Name"].Caption = "Ürün Adı";
                gridView1.Columns["Barcode"].Caption = "Barkod";
                gridView1.Columns["StockQuantity"].Caption = "Stok Miktarı";
                gridView1.Columns["Price"].Caption = "Fiyat";
                gridView1.Columns["Category"].Caption = "Kategori";
                gridView1.Columns["Brand"].Caption = "Marka";
                gridView1.Columns["ModelName"].Caption = "Model";
                gridView1.Columns["ModelYear"].Caption = "Model Yılı";
                gridView1.Columns["ShelfName"].Caption = "Raf";
                gridView1.Columns["IsActive"].Caption = "Durum";

                // İstenmeyen sütunları gizle
                gridView1.Columns["Id"].Visible = false; // "Id" sütununu gizle
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ürünler yüklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void FrmUrunler_Load(object sender, EventArgs e)
        {
            LoadProducts();
        }
    }
}