using StokTakipApp.BusinessLayer.Concrete;
using StokTakipApp.DataAccessLayer.Context;
using StokTakipApp.DataAccessLayer.EntityFramework;
using StokTakipApp.EntityLayer.Concrete;
using StokTakipApp.PresentationLayer.Helpers;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace StokTakipApp.PresentationLayer.Modules.Tanımlar
{
    public partial class FrmKategori : DevExpress.XtraEditors.XtraForm
    {       
        private readonly CategoryManager _categoryManager = new CategoryManager(new EfCategoryDal(new AppDbContext()));
        private int _categoryId;
        public FrmKategori()
        {
            InitializeComponent();
        }
        private void LoadCategories()
        {
            try
            {
                gridControl1.DataSource = null; // İlk başta boş bir yapı
                gridControl1.DataSource = _categoryManager.TGetAll().Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Description,
                    IsActive = c.IsActive ? "Aktif" : "Pasif" // Durum için string değer
                }).ToList(); // Kategorileri listele);
                gridView1.Columns["Name"].Caption = "Kategori Adı";
                gridView1.Columns["Description"].Caption = "Açıklama";
                gridView1.Columns["IsActive"].Caption = "Durum";
                gridView1.Columns["Id"].Visible = false; // "Id" sütununu gizle
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, ExceptionHandler.LogLevel.Error);
            }
        }
        private void ClearFields()
        {
            txtAd.Text = string.Empty;            
            tsDurum.IsOn = true;
            txtAciklama.Text = string.Empty;
            _categoryId = 0; // ID'yi sıfırla
            btnKaydet.Enabled = true;
            BtnGuncelle.Enabled = false;
            BtnSil.Enabled = false;
        }

        private void txtAd_EditValueChanged(object sender, EventArgs e)
        {
            // Eğer txtAd boşsa ve yeni kategori ekleme modundaysa (_categoryId == 0)
            if (string.IsNullOrWhiteSpace(txtAd.Text) && _categoryId == 0)
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
                if (string.IsNullOrWhiteSpace(txtAd.Text) || _categoryId != 0)
                {
                    btnKaydet.Enabled = false;
                    BtnGuncelle.Enabled = true;
                    BtnSil.Enabled = true;
                }
            }
        }
        private void FrmKategori_Load(object sender, EventArgs e)
        {
            LoadCategories();            
            txtAd_EditValueChanged(null, null); // Başlangıçta butonları pasif yap
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidationHelper.ValidateControl(txtAd, "Kategori adı boş bırakılamaz!")) return;
                // Aynı isimde kategori olup olmadığını kontrol et
                var existingCategory = _categoryManager.TGetAll()
                    .FirstOrDefault(c => c.Name.Equals(txtAd.Text.Trim(), StringComparison.OrdinalIgnoreCase));
                if (existingCategory != null)
                {
                    MessageBox.Show($"{txtAd.Text.Trim()} kategorisi zaten mevcut!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Yeni kategori oluştur ve bilgileri ekle
                var category = new Category
                {
                    Name = txtAd.Text.Trim(),
                    Description = txtAciklama.Text.Trim(),
                    IsActive = tsDurum.IsOn
                };
                _categoryManager.TInsert(category);

                MessageBox.Show("Kategori başarıyla kaydedildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Listeyi yenile ve form alanlarını temizle
                LoadCategories();
                ClearFields(); // Alanları temizle
            }
            catch (Exception ex)
            {
                // Hata yönetimi
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ExceptionHandler.HandleException(ex, ExceptionHandler.LogLevel.Error);

            }
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidationHelper.ValidateControl(txtAd, "Kategori adı boş bırakılamaz!")) return;
                // Aynı isim kontrolü, ID farklı olan kayıtlar için
                var existingCategory = _categoryManager.TGetAll()
                    .FirstOrDefault(c => c.Name.Equals(txtAd.Text.Trim(), StringComparison.OrdinalIgnoreCase) && c.Id != _categoryId);

                if (existingCategory != null)
                {
                    MessageBox.Show($"{txtAd.Text.Trim()} kategorisi zaten mevcut!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Güncellenecek kategori bilgilerini al
                var category = _categoryManager.TGetById(_categoryId);
                if (category != null)
                {
                    category.Name = txtAd.Text.Trim();
                    category.Description = txtAciklama.Text.Trim();
                    category.IsActive = tsDurum.IsOn;

                    // Kategori bilgilerini güncelle
                    _categoryManager.TUpdate(category);

                    MessageBox.Show("Kategori başarıyla güncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Verileri yeniden yükle ve alanları temizle
                    LoadCategories();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Güncellenecek kategori bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Hata yönetimi
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ExceptionHandler.HandleException(ex, ExceptionHandler.LogLevel.Error);
            }
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidationHelper.ValidateControl(txtAd, "Kategori adı boş bırakılamaz!")) return;
                // Silme işlemi için kategori seçimini kontrol et
                if (_categoryId == 0)
                {
                    MessageBox.Show("Lütfen silmek istediğiniz kategoriyi seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kullanıcıdan silme işlemi onayı al
                var result = MessageBox.Show($"'{txtAd.Text}' kategorisini silmek istediğinize emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Mevcut kategori bilgilerini al ve pasif hale getir
                    var category = _categoryManager.TGetById(_categoryId);
                    if (category != null)
                    {
                        category.IsActive = false; // Kategori pasif hale getiriliyor
                        _categoryManager.TUpdate(category); // Güncelleme işlemi

                        MessageBox.Show("Kategori başarıyla silindi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Listeyi yenile ve alanları temizle
                        LoadCategories();
                        ClearFields();
                    }
                    else
                    {
                        MessageBox.Show("Kategori bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                // Çift tıklanan yerin bir satır olup olmadığını kontrol et
                if (gridView1.FocusedRowHandle >= 0) // FocusedRowHandle -1 ise, hiçbir satır seçili değildir
                {
                    var selectedCategoryId = (int)gridView1.GetFocusedRowCellValue("Id");
                    var category = _categoryManager.TGetById(selectedCategoryId);

                    if (category != null)
                    {
                        // Seçilen kategorinin bilgilerini form alanlarına aktar
                        _categoryId = category.Id; // Seçilen kategorinin ID'sini alıyoruz
                        txtAd.Text = category.Name;
                        txtAciklama.Text = category.Description;
                        tsDurum.IsOn = category.IsActive;
                    }
                    else
                    {
                        // Eğer kategori bulunamazsa hata mesajı
                        MessageBox.Show("Seçilen kategori bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Eğer hiçbir satır seçilmemişse bilgilendirme mesajı
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
