using StokTakipApp.PresentationLayer.Modules.Products;
using StokTakipApp.PresentationLayer.Modules.Tanımlar;

namespace StokTakipApp.PresentationLayer
{
    partial class FrmDashboard : DevExpress.XtraEditors.XtraForm
    {
        public FrmDashboard()
        {
            InitializeComponent();
        }      
               
        private void FrmDashboard_Load(object sender, System.EventArgs e)
        {

        }
        FrmKategori frmKategori;
        private void btnKategoriT_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (frmKategori == null || frmKategori.IsDisposed)
            {
                frmKategori = new FrmKategori();
                frmKategori.MdiParent = this;
                frmKategori.Show();
            }
            else
            {
                frmKategori.BringToFront();
            }
        }
        FrmMarka frmMarka;
        private void btnMarkaT_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (frmMarka == null || frmMarka.IsDisposed)
            {
                frmMarka = new FrmMarka();
                frmMarka.MdiParent = this;
                frmMarka.Show();
            }
            else
            {
                frmMarka.BringToFront();
            }
        }
        FrmModel frmModel;
        private void btnModelT_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (frmModel == null || frmModel.IsDisposed)
            {
                frmModel = new FrmModel();
                frmModel.MdiParent = this;
                frmModel.Show();
            }
            else
            {
                frmModel.BringToFront();
            }
        }
        FrmRaf frmRaf;
        private void btnRafT_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (frmRaf == null || frmRaf.IsDisposed)
            {
                frmRaf = new FrmRaf();
                frmRaf.MdiParent = this;
                frmRaf.Show();
            }
            else
            {
                frmRaf.BringToFront();
            }
        }      
        FrmUrunler frmUrunler;
        private void btnUrunI_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (frmUrunler == null || frmUrunler.IsDisposed)
            {
                frmUrunler = new FrmUrunler();
                frmUrunler.MdiParent = this;
                frmUrunler.Show();
            }
            else
            {
                frmUrunler.BringToFront();
            }
        }
        FrmStoklar frmStoklar;
        private void btnStokD_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (frmStoklar == null || frmStoklar.IsDisposed)
            {
                frmStoklar = new FrmStoklar();
                frmStoklar.MdiParent = this;
                frmStoklar.Show();
            }
            else
            {
                frmStoklar.BringToFront();
            }
        }
    }
}