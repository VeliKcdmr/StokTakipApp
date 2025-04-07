using StokTakipApp.PresentationLayer.Helpers;
using StokTakipApp.BusinessLayer.Concrete;
using System;
using StokTakipApp.DataAccessLayer.EntityFramework;
using StokTakipApp.DataAccessLayer.Context;
using System.Linq;
using System.Windows.Forms;
using StokTakipApp.EntityLayer.Concrete;
using System.Collections.Generic;
using System.Drawing;


namespace StokTakipApp.PresentationLayer.Modules.Tanımlar
{
    public partial class FrmRaf : DevExpress.XtraEditors.XtraForm
    {
        ShelfManager _shelfManager = new ShelfManager(new EfShelfDal(new AppDbContext()));
        public int _shelfId; // Seçilen raf ID'si

        public FrmRaf()
        {
            InitializeComponent();
        }

        // Rafları yükleme
        void LoadShelves()
        {
            try
            {
                gridControl1.DataSource = null; // İlk başta boş bir yapı
                gridControl1.DataSource = _shelfManager.TGetAll().Select(c => new
                {
                    c.Id,
                    c.ShelfName,
                    IsActive = c.IsActive ? "Aktif" : "Pasif" // Durum için string değer
                }).ToList(); // Kategorileri listele);
                gridView1.Columns["ShelfName"].Caption = "Raf Adı";               
                gridView1.Columns["IsActive"].Caption = "Durum";
                gridView1.Columns["Id"].Visible = false; // "Id" sütununu gizle
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Raflar yüklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ExceptionHandler.HandleException(ex, ExceptionHandler.LogLevel.Error);
            }
        }
        private void txtAd_EditValueChanged(object sender, EventArgs e)
        {
            // Eğer txtAd boşsa ve yeni kategori ekleme modundaysa (_categoryId == 0)
            if (string.IsNullOrWhiteSpace(txtAd.Text) && _shelfId == 0)
            {
                btnKaydet.Enabled = true;
                BtnGuncelle.Enabled = false;
                BtnSil.Enabled = false;
            }
            else
            {
                // Varsayılan stil ve renk ayarları
                txtAd.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
                txtAd.Properties.Appearance.BorderColor = Color.Black;

                // Eğer txtAd boşsa veya mevcut bir kategori düzenleniyorsa (_categoryId != 0)
                if (string.IsNullOrWhiteSpace(txtAd.Text) || _shelfId != 0)
                {
                    btnKaydet.Enabled = false;
                    BtnGuncelle.Enabled = true;
                    BtnSil.Enabled = true;
                }
            }
        }

        private void ClearFields()
        {
            txtAd.Text = string.Empty;
            tsDurum.IsOn = true;
            txtAciklama.Text = string.Empty;
            _shelfId = 0; // ID'yi sıfırla
            btnKaydet.Enabled = true;
            BtnGuncelle.Enabled = false;
            BtnSil.Enabled = false;
        }

        // Form yüklendiğinde çalışacak
        private void FrmRaf_Load(object sender, EventArgs e)
        {
            LoadShelves(); // Rafları yükle
            txtAd_EditValueChanged(null, null); // Başlangıçta butonları pasif yap
        }

        // Yeni raf kaydetme
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidationHelper.ValidateControl(txtAd, "Raf adı boş bırakılamaz!")) return;

                var existingShelf = _shelfManager.TGetAll()
                    .FirstOrDefault(s => s.ShelfName.Equals(txtAd.Text.Trim(), StringComparison.OrdinalIgnoreCase));
                if (existingShelf != null)
                {
                    MessageBox.Show($"'{txtAd.Text.Trim()}' adlı raf zaten mevcut!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var shelf = new Shelf
                {
                    ShelfName = txtAd.Text.Trim(),
                    IsActive = true, // Varsayılan olarak aktif
                    Products = new List<Product>() // Boş ürün listesi
                };

                _shelfManager.TInsert(shelf);
                MessageBox.Show("Raf başarıyla kaydedildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadShelves();
                ClearFields(); // Form alanlarını temizleme
            }
            catch (Exception ex)
            {
                // Hata yönetimi
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ExceptionHandler.HandleException(ex, ExceptionHandler.LogLevel.Error);
            }

        }

        // Raf güncelleme
        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidationHelper.ValidateControl(txtAd, "Raf adı boş bırakılamaz!")) return;

                var existingShelf = _shelfManager.TGetAll()
                    .FirstOrDefault(s => s.ShelfName.Equals(txtAd.Text.Trim(), StringComparison.OrdinalIgnoreCase) && s.Id != _shelfId);
                if (existingShelf != null)
                {
                    MessageBox.Show($"'{txtAd.Text.Trim()}' adlı raf zaten mevcut!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var shelf = _shelfManager.TGetById(_shelfId);
                if (shelf != null)
                {
                    shelf.ShelfName = txtAd.Text.Trim();
                    shelf.IsActive = tsDurum.IsOn;

                    _shelfManager.TUpdate(shelf);
                    MessageBox.Show("Raf başarıyla güncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadShelves();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Güncellenecek raf bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Hata yönetimi
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ExceptionHandler.HandleException(ex, ExceptionHandler.LogLevel.Error);
            }

        }

        // Raf silme
        private void BtnSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidationHelper.ValidateControl(txtAd, "Raf adı boş bırakılamaz!")) return;

                var result = MessageBox.Show($"'{txtAd.Text}' adlı rafı silmek istediğinize emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    var shelf = _shelfManager.TGetById(_shelfId);
                    if (shelf != null)
                    {
                        shelf.IsActive = false; // Pasif hale getir
                        _shelfManager.TUpdate(shelf);

                        MessageBox.Show("Raf başarıyla silindi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadShelves();
                        ClearFields();
                    }
                    else
                    {
                        MessageBox.Show("Silmek istediğiniz raf bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata yönetimi
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ExceptionHandler.HandleException(ex, ExceptionHandler.LogLevel.Error);
            }

        }

        // GridView çift tıklama işlemi
        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (gridView1.FocusedRowHandle >= 0)
                {
                    var selectedShelfId = (int)gridView1.GetFocusedRowCellValue("Id");
                    var shelf = _shelfManager.TGetById(selectedShelfId);

                    if (shelf != null)
                    {
                        _shelfId = shelf.Id;
                        txtAd.Text = shelf.ShelfName;
                        tsDurum.IsOn = shelf.IsActive;
                    }
                    else
                    {
                        MessageBox.Show("Seçilen raf bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Lütfen bir satır seçiniz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Hata yönetimi
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ExceptionHandler.HandleException(ex, ExceptionHandler.LogLevel.Error);
            }

        }       
    }
}