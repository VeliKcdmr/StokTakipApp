using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using System;

namespace StokTakipApp.PresentationLayer
{
    public partial class SplashScreen1 : SplashScreen
    {
        public SplashScreen1()
        {
            InitializeComponent();
            this.labelCopyright.Text = "Copyright © 2024-" + DateTime.Now.Year.ToString();
        }

        #region Overrides

        // Gelen komutları işlemek için ProcessCommand metodu
        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);

            SplashScreenCommand command = (SplashScreenCommand)cmd; // Enum değerine dönüştür
            if (command == SplashScreenCommand.UpdateProgress)
            {
                int progress = Convert.ToInt32(arg); // Argümanı int'e dönüştür
                progressBarControl.EditValue = progress; // Progress bar değerini güncelle
                labelStatus.Text = $"Yükleniyor: %{progress}"; // Durum mesajını göster
            }
            else if (command == SplashScreenCommand.ShowMessage)
            {
                labelStatus.Text = $"Mesaj: {arg}"; // Gelen mesajı göster
            }
            else if (command == SplashScreenCommand.Complete)
            {
                labelStatus.Text = "Yükleme tamamlandı!"; // Yükleme tamamlandı mesajı
                progressBarControl.EditValue = 100; // Progress bar tam dolu
            }
        }

        #endregion

        // SplashScreen ile kullanılacak komutlar
        public enum SplashScreenCommand
        {
            UpdateProgress, // İlerleme durumu
            ShowMessage,    // Mesaj göster
            Complete        // Yükleme tamamlandı
        }

        private void SplashScreen1_Load(object sender, EventArgs e)
        {
            // SplashScreen yüklendiğinde yapılacak işlemler
        }
    }
}