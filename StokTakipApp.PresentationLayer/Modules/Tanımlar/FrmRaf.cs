using StokTakipApp.PresentationLayer.Helpers;
using StokTakipApp.BusinessLayer.Concrete;
using System;
using StokTakipApp.DataAccessLayer.EntityFramework;
using StokTakipApp.DataAccessLayer.Context;


namespace StokTakipApp.PresentationLayer.Modules.Tanımlar
{
    public partial class FrmRaf : DevExpress.XtraEditors.XtraForm
    {
        ShelfManager _shelfManager= new ShelfManager(new EfShelfDal(new AppDbContext()));
        public FrmRaf()
        {
            InitializeComponent();
        }
        void LoadShelves()
        {
            gridControl1.DataSource = _shelfManager.TGetAll();
            gridView1.BestFitColumns();
        }

        private void FrmRaf_Load(object sender, EventArgs e)
        {

        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (!ValidationHelper.ValidateControl(txtAd, "Kategori adı boş bırakılamaz!")) return;

        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            if (!ValidationHelper.ValidateControl(txtAd, "Kategori adı boş bırakılamaz!")) return;

        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            if (!ValidationHelper.ValidateControl(txtAd, "Kategori adı boş bırakılamaz!")) return;

        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {

        }
    }
}