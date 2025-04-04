using System;
using System.IO;
using System.Windows.Forms;

public static class ExceptionHandler
{
    public static void HandleException(Exception ex)
    {
        // Log dosyasının yolu
        string logFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "StokTakipErrorlog.txt"
        );

        // Dosya yoksa oluştur
        if (!File.Exists(logFilePath))
        {
            File.Create(logFilePath).Close();
        }

        // Dosyaya yazma işlemi
        using (var writer = new StreamWriter(logFilePath, true))
        {
            writer.WriteLine($"Tarih: {DateTime.Now}");
            writer.WriteLine($"Hata Mesajı: {ex.Message}");
            writer.WriteLine($"Hata Detayları: {ex.StackTrace}");
            writer.WriteLine(new string('-', 50)); // Bölme çizgisi
        }

        // Kullanıcıya hata mesajını göster
        MessageBox.Show(
            "Beklenmedik bir hata oluştu. Daha fazla bilgi için log dosyasını kontrol edin.",
            "Hata",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        );
    }
}