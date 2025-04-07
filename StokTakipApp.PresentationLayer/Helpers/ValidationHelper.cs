using System.Drawing;
using System.Windows.Forms;

namespace StokTakipApp.PresentationLayer.Helpers
{
    public static class ValidationHelper
    {
        public static bool ValidateControl<T>(T control, string errorMessage) where T : DevExpress.XtraEditors.BaseEdit
        {
            // textEdit için doğrulama

            if (control is DevExpress.XtraEditors.TextEdit && string.IsNullOrWhiteSpace((control as DevExpress.XtraEditors.TextEdit)?.Text))
            {
                MessageBox.Show(errorMessage, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                control.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
                control.Properties.Appearance.BorderColor = Color.Red;
                control.Focus();
                return false;
            }
            // ComboBoxEdit için doğrulama
            else if (control is DevExpress.XtraEditors.ComboBoxEdit && control.EditValue == null)
            {
                MessageBox.Show(errorMessage, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                control.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
                control.Properties.Appearance.BorderColor = Color.Red;
                return false;
            }
            // LookUpEdit için doğrulama
            else if (control is DevExpress.XtraEditors.LookUpEdit && (control as DevExpress.XtraEditors.LookUpEdit).EditValue == null)
            {
                MessageBox.Show(errorMessage, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                control.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
                control.Properties.Appearance.BorderColor = Color.Red;
                return false;
            }
            // DateTimeOffsetEdit için doğrulama
            else if (control is DevExpress.XtraEditors.DateTimeOffsetEdit && (control as DevExpress.XtraEditors.DateTimeOffsetEdit).EditValue == null)
            {
                MessageBox.Show(errorMessage, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                control.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
                control.Properties.Appearance.BorderColor = Color.Red;
                return false;
            }

            control.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            control.Properties.Appearance.BorderColor = Color.Black;
            return true;


        }
    }
}
