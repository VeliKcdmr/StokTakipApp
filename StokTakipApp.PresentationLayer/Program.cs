using DevExpress.XtraSplashScreen;
using StokTakipApp.DataAccessLayer;
using StokTakipApp.DataAccessLayer.Context;
using System;
using System.Linq;
using System.Windows.Forms;

namespace StokTakipApp.PresentationLayer
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // SplashScreen'i başlat
            SplashScreenManager.ShowForm(typeof(SplashScreen1));

            // Ürünleri yükleme işlemi
            LoadProductsWithSplashScreen();

            // SplashScreen'i kapat
            SplashScreenManager.CloseForm();

            // Dashboard formunu başlat
            Application.Run(new FrmDashboard());
        }

        private static void LoadProductsWithSplashScreen()
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    if (!context.Database.Exists())
                    {
                        throw new Exception("Veritabanına bağlanılamıyor. Lütfen bağlantıyı kontrol edin.");
                    }

                    var products = context.Products.ToList(); // Ürünleri çek
                    int total = products.Count; // Toplam ürün sayısı

                    if (total == 0)
                    {
                        SplashScreenManager.Default.SendCommand(SplashScreen1.SplashScreenCommand.ShowMessage, "Gösterilecek ürün bulunamadı!");
                        System.Threading.Thread.Sleep(5000); // 2 saniye bekle
                        return;
                    }

                    int progress = 0;
                    foreach (var product in products)
                    {
                        System.Threading.Thread.Sleep(100); // Veri çekme simülasyonu

                        // İlerleme yüzdesini hesapla
                        progress += (100 / total);
                        if (progress > 100) progress = 100;

                        // SplashScreen'e ilerleme durumu gönder
                        SplashScreenManager.Default.SendCommand(SplashScreen1.SplashScreenCommand.UpdateProgress, progress);
                    }

                    // Ürünleri önbelleğe kaydet
                    CachedData.Products = products;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
                SplashScreenManager.Default.SendCommand(SplashScreen1.SplashScreenCommand.ShowMessage, "Veritabanı bağlantısı başarısız!");
                System.Threading.Thread.Sleep(5000); // 2 saniye bekle
            }
        }
    }
}
