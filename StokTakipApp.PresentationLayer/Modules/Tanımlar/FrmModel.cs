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
    public partial class FrmModel : DevExpress.XtraEditors.XtraForm
    {
        private readonly ModelManager _modelManager = new ModelManager(new EfModelDal(new AppDbContext()));
        private readonly BrandManager _brandManager = new BrandManager(new EfBrandDal(new AppDbContext()));
        private int _modelId; // Seçilen modelin ID'si
        public FrmModel()
        {
            InitializeComponent();
        }

        private void LoadModels()
        {
            try
            {
                // Marka listesini Dictionary olarak hazırlayın (Performans için)
                var brandsDict = _brandManager.TGetAll()
                    .ToDictionary(b => b.Id, b => b.Name); // Anahtar: Marka ID, Değer: Marka Adı

                // Modelleri listeye dönüştür ve ilgili markanın adını al
                var models = _modelManager.TGetAll()
                    .Select(m => new
                    {
                        m.Id,
                        m.Name,
                        m.Year,
                        BrandName = brandsDict.ContainsKey(m.BrandId) ? brandsDict[m.BrandId] : "Marka Bulunamadı",
                        m.Description,
                        IsActive = m.IsActive ? "Aktif" : "Pasif" // Durum bilgisini daha kullanıcı dostu yap
                    }).ToList();

                // GridControl veri kaynağı
                gridControl1.DataSource = models;

                // Sütun başlıklarını ayarla
                gridView1.Columns["Name"].Caption = "Model Adı";
                gridView1.Columns["Year"].Caption = "Model Yılı";
                gridView1.Columns["BrandName"].Caption = "Markası"; // Markanın adını göster
                gridView1.Columns["Description"].Caption = "Açıklama";
                gridView1.Columns["IsActive"].Caption = "Durum";
                gridView1.Columns["Id"].Visible = false; // "Id" sütununu gizle
            }
            catch (Exception ex)
            {
                // Hata loglama veya kullanıcıya bilgilendirme
                ExceptionHandler.HandleException(ex, ExceptionHandler.LogLevel.Error);
            }
        }
        private void LoadBrands()
        {
            try
            {
                // Kategorileri veritabanından al
                var brands = _brandManager.TGetAll().Where(c => c.IsActive).Select(c => new
                {
                    c.Id, // ID alanı
                    c.Name // Görüntülenecek kategori adı
                }).ToList();

                // LookupEdit'in veri kaynağını ayarla
                cmbMarka.Properties.DataSource = brands; // Veri kaynağını bağla
                cmbMarka.Properties.DisplayMember = "Name"; // Kullanıcıya gösterilecek alan
                cmbMarka.Properties.ValueMember = "Id"; // Arka planda tutulacak değer
                cmbMarka.Properties.NullText = "Marka Seçiniz"; // Boş değer metni
                cmbMarka.Properties.PopulateColumns(); // Sütunları doldur
                cmbMarka.Properties.Columns["Name"].Caption = "Marka Adı"; // Sütun başlığı
                cmbMarka.Properties.Columns["Id"].Visible = false; // ID sütununu gizle

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, ExceptionHandler.LogLevel.Error); ;
            }

        }
        private void ClearFields()
        {
            txtAd.Text = string.Empty;
            cmbMarka.EditValue = null;
            cmbModelYil.EditValue = null;
            tsDurum.IsOn = true;
            txtAciklama.Text = string.Empty;
            _modelId = 0; // ID'yi sıfırla
            btnKaydet.Enabled = true;
            BtnGuncelle.Enabled = false;
            BtnSil.Enabled = false;
        }
        private void txtAd_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAd.Text) && _modelId == 0)
            {
                btnKaydet.Enabled = true;
                BtnGuncelle.Enabled = false;
                BtnSil.Enabled = false;

            }
            else
            {
                txtAd.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
                txtAd.Properties.Appearance.BorderColor = Color.Black; // Varsayılan renk

                if (string.IsNullOrWhiteSpace(txtAd.Text) || _modelId != 0)
                {
                    btnKaydet.Enabled = false;
                    BtnGuncelle.Enabled = true;
                    BtnSil.Enabled = true;
                }
            }
        }

        private void FrmModel_Load(object sender, EventArgs e)
        {
            LoadModels();
            LoadBrands();
            txtAd_EditValueChanged(null, null);

        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                // Alanları kontrol et
                if (!ValidationHelper.ValidateControl(txtAd, "Model adı boş bırakılamaz!")) return;
                if (!ValidationHelper.ValidateControl(cmbModelYil, "Lütfen bir model yılı seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(cmbMarka, "Lütfen bir marka seçiniz!")) return;
                // Aynı isim ve yıl kontrolü
                var selectedYear = ((DateTimeOffset)cmbModelYil.EditValue).Year; // DateTimeOffset'ten yıl bilgisi
                var selectedBrandId = (int)cmbMarka.EditValue; // LookUpEdit'ten seçilen marka ID'si
                if (_modelManager.TGetAll().Any(c => c.Name == txtAd.Text.Trim() && c.Year == selectedYear && c.BrandId == selectedBrandId))
                {
                    MessageBox.Show($"'{txtAd.Text}' adlı model zaten {selectedYear} yılı için seçilen marka ile kayıtlı!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var model = new Model
                {
                    Name = txtAd.Text.Trim(),
                    Description = txtAciklama.Text,
                    Year = selectedYear, // Yıl bilgisi atanıyor
                    IsActive = tsDurum.IsOn,
                    BrandId = selectedBrandId // Marka ID atanıyor
                };
                // Veriyi ekle
                _modelManager.TInsert(model);
                MessageBox.Show("Model başarıyla kaydedildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Listeyi yenile ve alanları temizle
                LoadModels();
                ClearFields();
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
                // Alanları kontrol et
                if (!ValidationHelper.ValidateControl(txtAd, "Model adı boş bırakılamaz!")) return;
                if (!ValidationHelper.ValidateControl(cmbModelYil, "Lütfen bir model yılı seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(cmbMarka, "Lütfen bir marka seçiniz!")) return;
                var selectedYear = ((DateTimeOffset)cmbModelYil.EditValue).Year; // DateTimeOffset'ten yıl bilgisi
                var selectedBrandId = (int)cmbMarka.EditValue; // LookUpEdit'ten seçilen marka ID'si
                // Aynı isim ve yıl kombinasyonunu kontrol et
                if (_modelManager.TGetAll().Any(c => c.Name == txtAd.Text.Trim() && c.Year == selectedYear && c.Id != _modelId))
                {
                    MessageBox.Show($"'{txtAd.Text}' adlı model zaten {selectedYear} yılı için kayıtlı!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Güncellenmiş modeli oluştur
                var model = _modelManager.TGetById(_modelId);
                if (model != null)
                {
                    model.Id = _modelId;
                    model.Name = txtAd.Text.Trim();
                    model.Description = txtAciklama.Text;
                    model.Year = selectedYear; // Yıl bilgisi atanıyor
                    model.IsActive = tsDurum.IsOn;
                    model.BrandId = selectedBrandId; // Marka ID atanıyor                    
                    _modelManager.TUpdate(model);

                    MessageBox.Show("Model başarıyla güncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Listeyi yenile ve alanları temizle
                    LoadModels();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Güncellenecek model bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
                if (!ValidationHelper.ValidateControl(cmbModelYil, "Lütfen bir model yılı seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(cmbMarka, "Lütfen bir marka seçiniz!")) return;
                // Silme işlemi için model seçimini kontrol et
                if (_modelId == 0)
                {
                    MessageBox.Show("Lütfen silmek istediğiniz modeli seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kullanıcıdan silme işlemi onayı al
                var result = MessageBox.Show($"'{txtAd.Text}' adlı modeli silmek istediğinize emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Mevcut model bilgilerini al ve pasif hale getir
                    var model = _modelManager.TGetById(_modelId);
                    if (model != null)
                    {
                        model.IsActive = false; // Model pasif hale getiriliyor
                        _modelManager.TUpdate(model); // Güncelleme işlemi

                        MessageBox.Show("Model başarıyla silindi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Listeyi yenile ve alanları temizle
                        LoadModels();
                        ClearFields();
                    }
                    else
                    {
                        MessageBox.Show("Silmek istediğiniz model bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ExceptionHandler.HandleException(ex, ExceptionHandler.LogLevel.Error);
            }
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {

            try
            {
                // Çift tıklanan yerin bir satır olup olmadığını kontrol et
                if (gridView1.FocusedRowHandle >= 0) // FocusedRowHandle -1 ise, hiçbir satır seçili değildir
                {
                    var selectedModelId = (int)gridView1.GetFocusedRowCellValue("Id");
                    var model = _modelManager.TGetById(selectedModelId);

                    _modelId = model.Id; // Seçilen kategorinin ID'sini alıyoruz
                    txtAd.Text = model.Name;
                    cmbModelYil.EditValue = new DateTimeOffset(new DateTime(model.Year, 1, 1));// Seçilen kategori ID'sini alıyoruz
                    cmbMarka.EditValue = model.BrandId; // Seçilen kategori ID'sini alıyoruz
                    txtAciklama.Text = model.Description;
                    tsDurum.IsOn = model.IsActive;
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