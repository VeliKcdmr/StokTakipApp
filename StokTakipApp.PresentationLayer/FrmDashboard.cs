using StokTakipApp.PresentationLayer.Froms.Tanımlar;
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
                MessageBox.Show("Kategori formu zaten açık."); // Kullanıcıyı bilgilendirme
                frmKategori.BringToFront();
            }
        }
    }
}