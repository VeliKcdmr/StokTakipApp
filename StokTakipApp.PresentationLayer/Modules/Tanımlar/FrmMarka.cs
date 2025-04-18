﻿using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Customization;
using StokTakipApp.BusinessLayer.Concrete;
using StokTakipApp.DataAccessLayer.Context;
using StokTakipApp.DataAccessLayer.EntityFramework;
using StokTakipApp.EntityLayer.Concrete;
using StokTakipApp.PresentationLayer.Helpers;
using System;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace StokTakipApp.PresentationLayer.Modules.Tanımlar
{
    public partial class FrmMarka : DevExpress.XtraEditors.XtraForm
    {
        private readonly BrandManager _brandManager = new BrandManager(new EfBrandDal(new AppDbContext()));
        private readonly CategoryManager _categoryManager = new CategoryManager(new EfCategoryDal(new AppDbContext()));
        private int _brandId;
        public FrmMarka()
        {
            InitializeComponent();
        }

        private void LoadBrands()
        {
            try
            {
                var categories = _categoryManager.TGetAll().ToList();
                var brands = _brandManager.TGetAll()
                    .Select(b => new
                    {
                        b.Id,
                        b.Name,
                        CategoryName = categories.FirstOrDefault(c => c.Id == b.CategoryId).Name, // Kategori adını al 
                        b.Description,
                        IsActive = b.IsActive ? "Aktif" : "Pasif"
                    }).ToList();
                gridControl1.DataSource = brands;
                gridView1.Columns["Name"].Caption = "Marka Adı";
                gridView1.Columns["CategoryName"].Caption = "Kategorisi"; // Kategori adını göster
                gridView1.Columns["Description"].Caption = "Açıklama";
                gridView1.Columns["IsActive"].Caption = "Durum";
                gridView1.Columns["Id"].Visible = false; // "Id" sütununu gizle   
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, ExceptionHandler.LogLevel.Error); // Kendi ExceptionHandler sınıfınızı çağırıyoruz
            }
        }
        private void ClearFields()
        {
            txtAd.Text = string.Empty;
            cmbKategori.EditValue = null;
            tsDurum.IsOn = true;
            txtAciklama.Text = string.Empty;
            _brandId = 0; // ID'yi sıfırla
            btnKaydet.Enabled = true;
            BtnGuncelle.Enabled = false;
            BtnSil.Enabled = false;
        }
        private void LoadCategories()
        {
            try
            {
                // Kategorileri veritabanından al
                var categories = _categoryManager.TGetAll().Where(c => c.IsActive).Select(c => new
                {
                    c.Id, // ID alanı
                    c.Name // Görüntülenecek kategori adı
                }).ToList();

                // LookupEdit'in veri kaynağını ayarla
                cmbKategori.Properties.DataSource = categories; // Veri kaynağını bağla
                cmbKategori.Properties.DisplayMember = "Name"; // Kullanıcıya gösterilecek alan
                cmbKategori.Properties.ValueMember = "Id"; // Arka planda tutulacak değer
                cmbKategori.Properties.NullText = "Kategori Seçiniz"; // Boş değer metni
                cmbKategori.Properties.PopulateColumns(); // Sütunları doldur
                cmbKategori.Properties.Columns["Name"].Caption = "Kategori Adı"; // Sütun başlığı
                cmbKategori.Properties.Columns["Id"].Visible = false; // ID sütununu gizle
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, ExceptionHandler.LogLevel.Error); // Kendi ExceptionHandler sınıfınızı çağırıyoruz
            }
        }

        private void txtAd_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAd.Text) && _brandId == 0)
            {
                btnKaydet.Enabled = true;
                BtnGuncelle.Enabled = false;
                BtnSil.Enabled = false;

            }
            else
            {
                txtAd.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
                txtAd.Properties.Appearance.BorderColor = Color.Black; // Varsayılan renk

                if (string.IsNullOrWhiteSpace(txtAd.Text) || _brandId != 0)
                {
                    btnKaydet.Enabled = false;
                    BtnGuncelle.Enabled = true;
                    BtnSil.Enabled = true;
                }
            }
        }
        private void cmbKategori_EditValueChanged(object sender, EventArgs e)
        {
            cmbKategori.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            cmbKategori.Properties.Appearance.BorderColor = Color.Black; // Varsayılan renk
        }

        private void FrmMarka_Load(object sender, EventArgs e)
        {
            LoadBrands();
            LoadCategories();
            txtAd_EditValueChanged(null, null); // Başlangıçta butonları pasif yap
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidationHelper.ValidateControl(txtAd, "Model adı boş bırakılamaz!")) return;
                if (!ValidationHelper.ValidateControl(cmbKategori, "Lütfen bir marka seçiniz!")) return;

                // Aynı isim kontrolü

                var selectedCategoryId = (int)cmbKategori.EditValue;
                var existingBrand = _brandManager.TGetAll().FirstOrDefault(c => c.Name == txtAd.Text.Trim());

                if (existingBrand != null)
                {
                    MessageBox.Show(txtAd.Text + " Markası zaten mevcut!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var brand = new Brand
                {
                    Name = txtAd.Text.Trim(),
                    Description = txtAciklama.Text,
                    IsActive = tsDurum.IsOn,
                    CategoryId = selectedCategoryId // Seçilen kategori ID'sini al

                };
                _brandManager.TInsert(brand);
                MessageBox.Show("Marka başarıyla kaydedildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadBrands();
                LoadCategories();
                ClearFields(); // Alanları temizle
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ExceptionHandler.HandleException(ex, ExceptionHandler.LogLevel.Error);
            }
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidationHelper.ValidateControl(txtAd, "Model adı boş bırakılamaz!")) return;
                // Aynı isim kontrolü, ID farklı olan kayıtlar için
                var existingBrand = _brandManager.TGetAll()
                    .FirstOrDefault(c => c.Name.Equals(txtAd.Text.Trim(), StringComparison.OrdinalIgnoreCase) && c.Id != _brandId);

                if (existingBrand != null)
                {
                    MessageBox.Show(txtAd.Text.Trim() + " Marka zaten mevcut!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var brand = _brandManager.TGetById(_brandId);

                brand.Name = txtAd.Text;
                brand.Description = txtAciklama.Text;
                brand.IsActive = tsDurum.IsOn;
                brand.CategoryId = (int)cmbKategori.EditValue; // Seçilen kategori ID'sini al

                _brandManager.TUpdate(brand);

                MessageBox.Show("Marka başarıyla güncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCategories();
                LoadBrands();
                ClearFields(); // Alanları temizle
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ExceptionHandler.HandleException(ex, ExceptionHandler.LogLevel.Error);
            }
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidationHelper.ValidateControl(txtAd, "Model adı boş bırakılamaz!")) return;
                // Silme işlemi için marka seçimini kontrol et
                if (_brandId == 0)
                {
                    MessageBox.Show("Lütfen silmek istediğiniz markayı seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kullanıcıdan silme işlemi onayı al
                var result = MessageBox.Show($"'{txtAd.Text}' markasını silmek istediğinize emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Mevcut marka bilgilerini al ve pasif hale getir
                    var brand = _brandManager.TGetById(_brandId);
                    if (brand != null)
                    {
                        brand.IsActive = false; // Marka pasif hale getiriliyor
                        _brandManager.TUpdate(brand); // Güncelleme işlemi

                        MessageBox.Show("Marka başarıyla silindi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Listeyi yenile ve alanları temizle
                        LoadCategories();
                        LoadBrands();
                        ClearFields();
                    }
                    else
                    {
                        MessageBox.Show("Marka bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
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
                    var selectedBrandId = (int)gridView1.GetFocusedRowCellValue("Id");
                    var brand = _brandManager.TGetById(selectedBrandId);

                    _brandId = brand.Id; // Seçilen kategorinin ID'sini alıyoruz
                    txtAd.Text = brand.Name;
                    cmbKategori.EditValue = (int)brand.CategoryId; // Seçilen kategori ID'sini alıyoruz
                    txtAciklama.Text = brand.Description;
                    tsDurum.IsOn = brand.IsActive;
                }
                else
                {
                    MessageBox.Show("Lütfen bir satır seçiniz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ExceptionHandler.HandleException(ex, ExceptionHandler.LogLevel.Error);
            }
        }

    }
}