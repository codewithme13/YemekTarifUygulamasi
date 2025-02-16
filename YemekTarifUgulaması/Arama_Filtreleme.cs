using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace YemekTarifUgulaması
{
    public partial class Arama_Filtreleme : UserControl
    {
        public string connectionString = "Server=localhost;Port=5432;Database=mydatabase;Username=myuser;Password=mypassword";
        private bool tariflerGosteriliyor = false;

        public Arama_Filtreleme()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string aramaMetni = textBox1.Text.ToLower();

            // Veritabanından tüm verileri çek (performans için gerekliyse sadece filtrelenmiş verileri çek)
            string query = "SELECT * FROM dbo.\"Tariflers\" WHERE LOWER(\"TarifAdi\") LIKE @AramaMetni OR LOWER(\"Talimatlar\") LIKE @AramaMetni";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, connection))
                {
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@AramaMetni", "%" + aramaMetni + "%");

                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string comboBox2Value = comboBox2.SelectedItem?.ToString();
            string textBox3Value = textBox3.Text;
            string comboBox1Value = comboBox1.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(comboBox2Value) && !string.IsNullOrEmpty(textBox3Value) && !string.IsNullOrEmpty(comboBox1Value))
            {
                string listBoxItem = $"{comboBox2Value} - {textBox3Value} - {comboBox1Value}";
                listBox1.Items.Add(listBoxItem);

                // TextBox'ları ve ComboBox'ı temizle
                comboBox2.SelectedIndex = -1;
                textBox3.Clear();
                comboBox1.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.' && e.KeyChar != ','))
            {
                e.Handled = true;
                MessageBox.Show("Lütfen sayısal bir değer giriniz.", "Geçersiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if ((e.KeyChar == '.' || e.KeyChar == ',') && (sender as TextBox).Text.IndexOfAny(new char[] { '.', ',' }) > -1)
            {
                e.Handled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tariflerGosteriliyor = false;

            // ListBox'taki malzemeleri al
            var userMalzemeler = new List<string>();
            foreach (var item in listBox1.Items)
            {
                string[] parts = item.ToString().Split('-');
                string malzemeAdi = parts[0].Trim();
                userMalzemeler.Add(malzemeAdi);
            }

            // Tarifler tablosundaki tüm tarifleri ve malzemelerini al
            var tarifler = new Dictionary<int, (string TarifAdi, List<string> Malzemeler)>();
            string query =
                "SELECT t.\"TarifID\", t.\"TarifAdi\", m.\"MalzemeAdi\" " +
                "FROM dbo.\"Tariflers\" t " +
                "JOIN dbo.\"Tarif_Malzeme\" tm ON t.\"TarifID\" = tm.\"TarifID\" " +
                "JOIN dbo.\"Malzemelers\" m ON tm.\"MalzemeID\" = m.\"MalzemeID\"";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    NpgsqlCommand command = new NpgsqlCommand(query, connection);
                    NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    foreach (DataRow row in dataTable.Rows)
                    {
                        int tarifID = Convert.ToInt32(row["TarifID"]);
                        string tarifAdi = row["TarifAdi"].ToString();
                        string malzemeAdi = row["MalzemeAdi"].ToString();

                        if (!tarifler.ContainsKey(tarifID))
                        {
                            tarifler[tarifID] = (tarifAdi, new List<string>());
                        }
                        tarifler[tarifID].Malzemeler.Add(malzemeAdi);
                    }

                    // Benzerlik oranı hesapla
                    var benzerlikListesi = new List<(string TarifAdi, double BenzerlikOrani)>();
                    foreach (var tarif in tarifler)
                    {
                        int eslesenMalzemeSayisi = tarif.Value.Malzemeler
                            .Count(malzeme => userMalzemeler.Contains(malzeme));

                        double benzerlikOrani = (double)eslesenMalzemeSayisi / tarif.Value.Malzemeler.Count;
                        benzerlikListesi.Add((tarif.Value.TarifAdi, benzerlikOrani));
                    }

                    // Sonuçları sıralayıp DataGridView’e ekle
                    benzerlikListesi = benzerlikListesi.OrderByDescending(b => b.BenzerlikOrani).ToList();

                    DataTable resultTable = new DataTable();
                    resultTable.Columns.Add("TarifAdi");
                    resultTable.Columns.Add("Benzerlik Oranı");

                    foreach (var item in benzerlikListesi)
                    {
                        resultTable.Rows.Add(item.TarifAdi, $"{item.BenzerlikOrani:P2}"); // % formatında
                    }

                    dataGridView1.DataSource = resultTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Veritabanına bağlanırken bir hata oluştu: " + ex.Message);
                }
            }
        }


        private void Arama_Filtreleme_Load(object sender, EventArgs e)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT \"MalzemeAdi\" FROM dbo.\"Malzemelers\" ";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        comboBox2.Items.Clear();
                        while (reader.Read())
                        {
                            comboBox2.Items.Add(reader.GetString(0));
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tariflerGosteriliyor = true;

            // Tüm TextBox'ları temizle
            foreach (Control control in this.Controls)
            {
                if (control is TextBox textBox)
                {
                    textBox.Clear();
                }
            }

            // ComboBox'ları sıfırla
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;

            // ListBox'ı temizle
            listBox1.Items.Clear();

            // DataGridView'i temizle
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

            // Tariflers tablosunu listele
            string query = "SELECT * FROM dbo.\"Tariflers\"";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Tarifleri yüklerken bir hata oluştu: " + ex.Message);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (!tariflerGosteriliyor)
            {
                return;
            }

            if (e.RowIndex >= 0 && dataGridView1.Rows[e.RowIndex].Cells["TarifID"].Value != null)
            {
                int tarifID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["TarifID"].Value);

                string query = "SELECT m.\"MalzemeAdi\" " +
                               "FROM dbo.\"Tarif_Malzeme\" tm " +
                               "JOIN dbo.\"Malzemelers\" m ON tm.\"MalzemeID\" = m.\"MalzemeID\" " +
                               "WHERE tm.\"TarifID\" = @TarifID";

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@TarifID", tarifID);

                            using (NpgsqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    List<string> malzemeler = new List<string>();
                                    while (reader.Read())
                                    {
                                        malzemeler.Add(reader.GetString(0));
                                    }

                                    // Malzemeleri mesaj kutusunda göster
                                    MessageBox.Show(
                                        "Bu tarifin malzemeleri:\n" + string.Join("\n", malzemeler),
                                        "Malzemeler",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information
                                    );
                                }
                                else
                                {
                                    MessageBox.Show("Bu tarif için malzeme bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Malzemeleri yüklerken bir hata oluştu: " + ex.Message);
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen bir sıralama seçeneği seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string secim = comboBox3.SelectedItem.ToString();
            string query = "SELECT t.\"TarifAdi\", t.\"HazirlamaSuresi\", COALESCE(SUM(tm.\"MalzemeMiktar\" * m.\"BirimFiyat\"), 0) AS \"Maliyet\" " +
                           "FROM dbo.\"Tariflers\" t " +
                           "LEFT JOIN dbo.\"Tarif_Malzeme\" tm ON t.\"TarifID\" = tm.\"TarifID\" " +
                           "LEFT JOIN dbo.\"Malzemelers\" m ON tm.\"MalzemeID\" = m.\"MalzemeID\" " +
                           "GROUP BY t.\"TarifID\", t.\"TarifAdi\", t.\"HazirlamaSuresi\" ";

            switch (secim)
            {
                case "A-Z ye sırala":
                    query += "ORDER BY t.\"TarifAdi\" ASC";
                    break;
                case "Z-A ya sırala":
                    query += "ORDER BY t.\"TarifAdi\" DESC";
                    break;
                case "Hazırlanma Süresi (MAX)":
                    query += "ORDER BY t.\"HazirlamaSuresi\" DESC";
                    break;
                case "Hazırlanma Süresi (MIN)":
                    query += "ORDER BY t.\"HazirlamaSuresi\" ASC";
                    break;
                case "Maliyete göre ARTAN":
                    query += "ORDER BY \"Maliyet\" ASC";
                    break;
                case "Maliyete göre AZALAN":
                    query += "ORDER BY \"Maliyet\" DESC";
                    break;
                case "Malzeme sayısına göre":
                    query = "SELECT t.\"TarifAdi\", COUNT(tm.\"MalzemeID\") AS \"MalzemeSayisi\" " +
                            "FROM dbo.\"Tariflers\" t " +
                            "JOIN dbo.\"Tarif_Malzeme\" tm ON t.\"TarifID\" = tm.\"TarifID\" " +
                            "GROUP BY t.\"TarifID\", t.\"TarifAdi\" " +
                            "ORDER BY COUNT(tm.\"MalzemeID\") ASC";
                    break;
                case "Kategoriye göre":
                    query = "SELECT t.\"TarifAdi\", t.\"Kategori\" " +
                            "FROM dbo.\"Tariflers\" t " +
                            "ORDER BY t.\"Kategori\" ASC, t.\"TarifAdi\" ASC";
                    break;
                case "Maliyete göre ARALIK":
                    if (double.TryParse(textBox2.Text, out double minMaliyet) && double.TryParse(textBox4.Text, out double maxMaliyet))
                    {
                        query += $"HAVING COALESCE(SUM(tm.\"MalzemeMiktar\" * m.\"BirimFiyat\"), 0) BETWEEN {minMaliyet} AND {maxMaliyet}";
                    }
                    else
                    {
                        MessageBox.Show("Lütfen geçerli bir maliyet aralığı giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    break;
                default:
                    MessageBox.Show("Geçersiz sıralama seçeneği.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

            SorguCalistir(query);
        }
        private void SorguCalistir(string query)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    NpgsqlCommand command = new NpgsqlCommand(query, connection);
                    NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells["TarifAdi"].Value != null)
                        {
                            string tarifAdi = row.Cells["TarifAdi"].Value.ToString();
                            bool eksikMalzemeVarMi = false;

                            string malzemeQuery =
                                "SELECT m.\"MalzemeAdi\", m.\"ToplamMiktar\", tm.\"MalzemeMiktar\" " +
                                "FROM dbo.\"Tarif_Malzeme\" tm " +
                                "JOIN dbo.\"Malzemelers\" m ON tm.\"MalzemeID\" = m.\"MalzemeID\" " +
                                "WHERE tm.\"TarifID\" = (SELECT \"TarifID\" FROM dbo.\"Tariflers\" WHERE \"TarifAdi\" = @TarifAdi)";

                            using (NpgsqlCommand malzemeCommand = new NpgsqlCommand(malzemeQuery, connection))
                            {
                                malzemeCommand.Parameters.AddWithValue("@TarifAdi", tarifAdi);
                                using (NpgsqlDataReader malzemeReader = malzemeCommand.ExecuteReader())
                                {
                                    while (malzemeReader.Read())
                                    {
                                        double toplamMiktar = malzemeReader.GetDouble(1);
                                        double kullanilanMiktar = malzemeReader.GetDouble(2);
                                        if (toplamMiktar < kullanilanMiktar)
                                        {
                                            eksikMalzemeVarMi = true;
                                            break;
                                        }
                                    }
                                }
                            }

                            if (eksikMalzemeVarMi)
                            {
                                row.DefaultCellStyle.BackColor = Color.Red;
                            }
                            else
                            {
                                row.DefaultCellStyle.BackColor = Color.Green;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Sıralama işlemi sırasında bir hata oluştu: " + ex.Message);
                }
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && ((TextBox)sender).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.' && ((TextBox)sender).Text.IndexOf('.') > -1)
            {
                e.Handled = true; 
            }
        }
    }
}



