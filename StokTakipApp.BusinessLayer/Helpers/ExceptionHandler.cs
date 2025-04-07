using System;
using System.IO;
using System.Data.Entity;
using StokTakipApp.DataAccessLayer.Context;
using StokTakipApp.EntityLayer.Concrete;

public static class ExceptionHandler
{
    public enum LogLevel
    {
        Info,
        Warning,
        Error,
        Critical
    }

    public static void HandleException(Exception ex, LogLevel level)
    {
        try
        {
            // Log'u hem dosyaya hem veritabanına kaydet
            SaveToLogFile(ex, level);
            SaveToDatabase(ex, level);

            // Kullanıcıya mesaj göster
            System.Windows.Forms.MessageBox.Show(
                $"Bir hata oluştu. Daha fazla bilgi için log dosyasını (Logs klasörü) veya sistem yöneticinizle iletişime geçin.",
                "Hata",
                System.Windows.Forms.MessageBoxButtons.OK,
                System.Windows.Forms.MessageBoxIcon.Error
            );
        }
        catch (Exception loggingEx)
        {
            // Log kaydederken hata oluşursa dosyaya kaydedilmeye çalışılır
            try
            {
                SaveToLogFile(loggingEx, LogLevel.Critical);
            }
            catch
            {
                // Dosya loglama da başarısız olursa sadece mesaj gösterilir
            }

            System.Windows.Forms.MessageBox.Show(
                $"Log kaydedilirken bir hata oluştu: {loggingEx.Message}",
                "Hata",
                System.Windows.Forms.MessageBoxButtons.OK,
                System.Windows.Forms.MessageBoxIcon.Error
            );
        }
    }

    private static void SaveToLogFile(Exception ex, LogLevel level)
    {
        try
        {
            // Uygulama adı ile birlikte Logs klasörü yolu
            string appName = "StokTakipApp"; // Uygulama adınız
            string logDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                appName,
                "Logs"
            );
            Directory.CreateDirectory(logDirectory); // Klasörü oluştur

            // Dinamik dosya adı: tarih bazlı
            string logFilePath = Path.Combine(logDirectory, $"ErrorLog_{DateTime.Now:dd.MM.yyyy}.txt");

            // Log dosyasına yazma işlemi
            using (var writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"Log ID: {Guid.NewGuid()}"); // Benzersiz ID
                writer.WriteLine($"Tarih: {DateTime.Now}");
                writer.WriteLine($"Log Seviyesi: {level}");
                writer.WriteLine($"Hata Mesajı: {ex.Message}");
                writer.WriteLine($"Hata Kaynağı: {ex.Source ?? "Unknown"}");
                writer.WriteLine($"Hedef Site: {ex.TargetSite?.ToString() ?? "Unknown"}");
                writer.WriteLine($"Hata Detayları: {ex.StackTrace ?? "No StackTrace"}");
                writer.WriteLine(new string('-', 50)); // Bölme çizgisi
            }
        }
        catch (Exception fileEx)
        {
            // Dosya loglama sırasında hata olursa kullanıcıya bildirim gösterilir
            System.Windows.Forms.MessageBox.Show(
                $"Log dosyasına yazma sırasında bir hata oluştu: {fileEx.Message}",
                "Hata",
                System.Windows.Forms.MessageBoxButtons.OK,
                System.Windows.Forms.MessageBoxIcon.Error
            );
        }

    }

    public static void SaveToDatabase(Exception ex, LogLevel level)
    {
        try
        {
            using (var context = new AppDbContext())
            {
                // ErrorLog nesnesi oluşturuluyor
                var errorLog = new ErrorLog
                {
                    LogDate = DateTime.Now,
                    LogLevel = level.ToString(),
                    Message = ex.Message,
                    Source = ex.Source ?? "Unknown",
                    TargetSite = ex.TargetSite?.ToString() ?? "Unknown",
                    StackTrace = ex.StackTrace ?? "No StackTrace"
                };

                // ErrorLog tablosuna ekleme
                context.ErrorLogs.Add(errorLog);
                context.SaveChanges();
            }
        }
        catch (Exception dbEx)
        {
            // Veritabanına kaydedilemezse dosya loglama devreye girer
            SaveToLogFile(dbEx, LogLevel.Critical);
        }
    }
}