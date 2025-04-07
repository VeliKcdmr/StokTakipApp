using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace StokTakipApp.PresentationLayer.Modules
{
    public partial class FrmAyarlar : DevExpress.XtraEditors.XtraForm
    {
        public FrmAyarlar()
        {
            InitializeComponent();
        }

        // Mevcut ayarları okuyup formdaki alanları doldurma
        private void SqlOptionsRead()
        {
            try
            {
                // App.config'den bağlantı dizesini oku
                string connString = ConfigurationManager.ConnectionStrings["StokTakipCon"].ConnectionString;

                // Bağlantı dizesini parçala
                var connParts = ParseConnectionString(connString);

                // Alanları doldur
                txtServerName.Text = connParts["Server"];
                txtDatabaseName.Text = connParts["Database"];

                if (connParts.ContainsKey("User Id")) // SQL Server Authentication kullanılıyorsa
                {
                    rbSQLAuth.Checked = true;
                    txtUserName.Text = connParts["User Id"];
                    txtPassword.Text = connParts["Password"];
                    txtUserName.Enabled = true;
                    txtPassword.Enabled = true;
                }
                else // Windows Authentication kullanılıyorsa
                {
                    rbWindowsAuth.Checked = true;
                    txtUserName.Enabled = false;
                    txtPassword.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Mevcut bağlantı ayarları okunamadı: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Bağlantı dizesini parçalama metodu
        private Dictionary<string, string> ParseConnectionString(string connectionString)
        {
            var parts = connectionString.Split(';');
            var connDict = new Dictionary<string, string>();

            foreach (var part in parts)
            {
                var keyValue = part.Split('=');
                if (keyValue.Length == 2)
                {
                    connDict[keyValue[0].Trim()] = keyValue[1].Trim();
                }
            }

            return connDict;
        }

        // Form yüklenirken ayarları ve zamanlayıcıyı başlat
        private void FrmAyarlar_Load(object sender, EventArgs e)
        {
            SqlOptionsRead(); // Mevcut ayarları oku
            timer1.Interval = 5000; // Her 5 saniyede bir kontrol eder
            timer1.Enabled = true;
            timer1.Tick += timer1_Tick;
        }

        // Bağlantı kontrol metodu
        private bool IsDatabaseConnected()
        {
            string connString = ConfigurationManager.ConnectionStrings["StokTakipCon"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT DB_NAME()", conn))
                    {
                        var databaseName = cmd.ExecuteScalar();
                        return databaseName != null && databaseName.ToString() == "StokTakipDB"; // Veritabanı doğrulama
                    }
                }
                catch
                {
                    return false; // Bağlantı hatalı
                }
            }
        }

        // Zamanlayıcıyla bağlantı durumu kontrol etme
        private void timer1_Tick(object sender, EventArgs e)
        {
            bool isConnected = IsDatabaseConnected();

            if (isConnected)
            {
                lblBDurum.Text = " Aktif";
                lblBDurum.Appearance.ForeColor = Color.Green; // Yeşil renk
            }
            else
            {
                lblBDurum.Text = " Pasif! Lütfen ayarları kontrol ediniz.";
                lblBDurum.Appearance.ForeColor = Color.Red; // Kırmızı renk
            }
        }

        // Authentication türüne göre alanların yönetimi
        private void rbWindowsAuth_CheckedChanged(object sender, EventArgs e)
        {
            txtUserName.Enabled = false; // Windows Authentication seçildiğinde devre dışı
            txtPassword.Enabled = false;
        }

        private void rbSQLAuth_CheckedChanged(object sender, EventArgs e)
        {
            txtUserName.Enabled = true; // SQL Server Authentication seçildiğinde aktif
            txtPassword.Enabled = true;
        }

        // Bağlantı dizesi oluşturma
        private string BuildConnectionString()
        {
            string serverName = txtServerName.Text;
            string databaseName = txtDatabaseName.Text;

            if (rbWindowsAuth.Checked) // Windows Authentication için
            {
                return $"Server={serverName};Database={databaseName};Integrated Security=True;";
            }
            else if (rbSQLAuth.Checked) // SQL Server Authentication için
            {
                string userName = txtUserName.Text;
                string password = txtPassword.Text;
                return $"Server={serverName};Database={databaseName};User Id={userName};Password={password};";
            }

            return string.Empty; // Hiçbir bağlantı türü seçilmediyse
        }

        // Bağlantıyı test etme
        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            string connString = BuildConnectionString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    lblConnectionStatus.Text = "Bağlantı başarılı!";
                    lblConnectionStatus.ForeColor = Color.Green;
                }
                catch (Exception ex)
                {
                    // Hata mesajını düzenle ve satırlara böl
                    string errorMessage = ex.Message.Length > 100
                        ? ex.Message.Substring(0, 100) + "..." // Çok uzun mesajı kısalt
                        : ex.Message;

                    lblConnectionStatus.Text = $"Bağlantı başarısız:\n{errorMessage}";
                    lblConnectionStatus.ForeColor = Color.Red;

                }
            }
        }
    }
}