using StokTakipApp.BusinessLayer.Concrete;
using StokTakipApp.PresentationLayer.Modules.Tanımlar;
using System.Windows.Forms;

namespace StokTakipApp.PresentationLayer
{ 
    public partial class FrmDashboard : DevExpress.XtraEditors.XtraForm
    { 
        public FrmDashboard()
        {
            InitializeComponent();
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
    }
}