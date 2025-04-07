
using System;

namespace StokTakipApp.EntityLayer.Concrete
{
    public class ErrorLog
    {
        public int Id { get; set; } // Primary Key
        public DateTime LogDate { get; set; } // Hata Tarihi
        public string LogLevel { get; set; } // Log Seviyesi (Info, Warning, Error, Critical)
        public string Message { get; set; } // Hata Mesajı
        public string Source { get; set; } // Hata Kaynağı
        public string TargetSite { get; set; } // Hedef Metot
        public string StackTrace { get; set; } // Hata Detayları
    }
}
