using StokTakipApp.BusinessLayer.Concrete;
using StokTakipApp.DataAccessLayer.Context;
using StokTakipApp.DataAccessLayer.EntityFramework;
using StokTakipApp.EntityLayer.Concrete;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace StokTakipApp.PresentationLayer.Modules.Tanımlar
{
    public partial class FrmKategori : DevExpress.XtraEditors.XtraForm
    {
        private readonly CategoryManager _categoryManager;
        private int _categoryId;

        public FrmKategori()
        {
            InitializeComponent();
            _categoryManager = new CategoryManager(new EfCategoryDal(new AppDbContext()));
        }
        private void ClearFields()
        {
            txtAd.Text = string.Empty;
            txtAciklama.Text = string.Empty;
            tsDurum.IsOn = true;
            _categoryId = 0; // Kategori ID'sini sıfırla
        }
        private void txtAd_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAd.Text) && _categoryId == 0)
            {
                btnKaydet.Enabled = true;
                BtnGuncelle.Enabled = false;
                BtnSil.Enabled = false;

            }
            else
            {
                txtAd.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
                txtAd.Properties.Appearance.BorderColor = Color.Black; // Varsayılan renk

                if (txtAd.Text != null && _categoryId != 0)
                {
                    btnKaydet.Enabled = false;
                    BtnGuncelle.Enabled = true;
                    BtnSil.Enabled = true;
                }
            }

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
                ExceptionHandler.HandleException(ex); // Kendi ExceptionHandler sınıfınızı çağırıyoruz
            }
        }

        private void FrmKategori_Load(object sender, EventArgs e)
        {
            txtAd.Text = string.Empty;
            LoadCategories();
            txtAd_EditValueChanged(null, null); // Başlangıçta butonları etkinleştir
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtAd.Text))
                {
                    MessageBox.Show("Kategori adı boş bırakılamaz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    // Hata durumunda textbox'ı kırmızı yap
                    txtAd.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
                    txtAd.Properties.Appearance.BorderColor = Color.Red;
                    txtAd.Focus();
                    return;
                }

                var existingCategory = _categoryManager.TGetAll().FirstOrDefault(c => c.Name == txtAd.Text.Trim());
                if (existingCategory != null)
                {
                    MessageBox.Show(txtAd.Text + " kategori zaten mevcut!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var category = new Category
                {
                    Name = txtAd.Text.Trim(),
                    Description = txtAciklama.Text,
                    IsActive = tsDurum.IsOn
                };
                _categoryManager.TInsert(category);
                MessageBox.Show("Kategori başarıyla kaydedildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCategories();
                ClearFields(); // Alanları temizle
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtAd.Text))
                {
                    MessageBox.Show("Kategori adı boş bırakılamaz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Aynı isim kontrolü, ID farklı olan kayıtlar için
                var existingCategory = _categoryManager.TGetAll()
                    .FirstOrDefault(c => c.Name.Equals(txtAd.Text.Trim(), StringComparison.OrdinalIgnoreCase) && c.Id != _categoryId);


                if (existingCategory != null)
                {
                    MessageBox.Show(txtAd.Text.Trim() + " kategori zaten mevcut!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var category = _categoryManager.TGetById(_categoryId);

                category.Name = txtAd.Text;
                category.Description = txtAciklama.Text;
                category.IsActive = tsDurum.IsOn;

                _categoryManager.TUpdate(category);

                MessageBox.Show("Kategori başarıyla güncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCategories();
                ClearFields(); // Alanları temizle
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MessageBox.Show(txtAd.Text + " kategoriyi silmek istediğinize emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    var category = _categoryManager.TGetById(_categoryId);
                    category.IsActive = false; // Silme işlemi yerine durumu pasif yapıyoruz
                    _categoryManager.TUpdate(category); // Güncelleme işlemi yapıyoruz

                    MessageBox.Show("Kategori başarıyla silindi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCategories();
                    ClearFields(); // Alanları temizle
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
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

                    _categoryId = category.Id; // Seçilen kategorinin ID'sini alıyoruz
                    txtAd.Text = category.Name;
                    txtAciklama.Text = category.Description;
                    tsDurum.IsOn = category.IsActive;
                }
                else
                {
                    MessageBox.Show("Lütfen bir satır seçiniz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

    }
}