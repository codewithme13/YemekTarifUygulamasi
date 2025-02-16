using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Npgsql;
using System.Drawing;

namespace YemekTarifUgulaması
{
    public partial class Tarif_Yönetimi : UserControl
    {
        private string connectionString = "Server=localhost;Port=5432;Database=mydatabase;Username=myuser;Password=mypassword";

        public Tarif_Yönetimi()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string query = "SELECT \"TarifID\", \"TarifAdi\", \"Kategori\", \"HazirlamaSuresi\", \"Talimatlar\" FROM dbo.\"Tariflers\"";
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
                        if (row.Cells["TarifID"].Value != null)
                        {
                            int tarifID = Convert.ToInt32(row.Cells["TarifID"].Value);
                            bool eksikMalzemeVarMi = false;
                            double eksikMalzemelerMaliyeti = 0;

                            string malzemeQuery =
                                "SELECT m.\"MalzemeAdi\", m.\"ToplamMiktar\", tm.\"MalzemeMiktar\", m.\"BirimFiyat\" " +
                                "FROM dbo.\"Tarif_Malzeme\" tm " +
                                "JOIN dbo.\"Malzemelers\" m ON tm.\"MalzemeID\" = m.\"MalzemeID\" " +
                                "WHERE tm.\"TarifID\" = @TarifID";

                            using (NpgsqlCommand malzemeCommand = new NpgsqlCommand(malzemeQuery, connection))
                            {
                                malzemeCommand.Parameters.AddWithValue("@TarifID", tarifID);
                                using (NpgsqlDataReader malzemeReader = malzemeCommand.ExecuteReader())
                                {
                                    while (malzemeReader.Read())
                                    {
                                        double toplamMiktar = malzemeReader.GetDouble(1);
                                        double kullanilanMiktar = malzemeReader.GetDouble(2);
                                        double birimFiyat = malzemeReader.GetDouble(3);

                                        if (toplamMiktar < kullanilanMiktar)
                                        {
                                            eksikMalzemeVarMi = true;
                                            eksikMalzemelerMaliyeti += (kullanilanMiktar - toplamMiktar) * birimFiyat;
                                        }
                                    }
                                }
                            }

                            if (eksikMalzemeVarMi)
                            {
                                row.DefaultCellStyle.BackColor = Color.Red;
                                row.Tag = eksikMalzemelerMaliyeti; 
                            }
                            else
                            {
                                row.DefaultCellStyle.BackColor = Color.Green;
                                row.Tag = 0;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Veritabanına bağlanırken bir hata oluştu: " + ex.Message);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                int tarifID = Convert.ToInt32(row.Cells["TarifID"].Value);

                double toplamMaliyet = 0.0;
                double eksikMalzemelerMaliyeti = 0.0;
                List<string> eksikMalzemeListesi = new List<string>();

                string maliyetQuery =
                    "SELECT m.\"MalzemeAdi\", m.\"ToplamMiktar\", tm.\"MalzemeMiktar\", m.\"BirimFiyat\" " +
                    "FROM dbo.\"Tarif_Malzeme\" tm " +
                    "JOIN dbo.\"Malzemelers\" m ON tm.\"MalzemeID\" = m.\"MalzemeID\" " +
                    "WHERE tm.\"TarifID\" = @TarifID";

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        using (NpgsqlCommand command = new NpgsqlCommand(maliyetQuery, connection))
                        {
                            command.Parameters.AddWithValue("@TarifID", tarifID);
                            using (NpgsqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string malzemeAdi = reader.GetString(0);
                                    double toplamMiktar = reader.GetDouble(1);
                                    double malzemeMiktar = reader.GetDouble(2);
                                    double birimFiyat = reader.GetDouble(3);

                                    toplamMaliyet += malzemeMiktar * birimFiyat;

                                    if (toplamMiktar < malzemeMiktar)
                                    {
                                        double eksikMiktar = malzemeMiktar - toplamMiktar;
                                        eksikMalzemeListesi.Add($"{malzemeAdi}");
                                        eksikMalzemelerMaliyeti += eksikMiktar * birimFiyat;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Maliyet hesaplanırken bir hata oluştu: " + ex.Message);
                        return;
                    }
                }

                string eksikMalzemeler = string.Join("\n", eksikMalzemeListesi);

                if (row.DefaultCellStyle.BackColor == Color.Red)
                {
                    MessageBox.Show($"Tarifinizin maliyeti: {toplamMaliyet:F2} TL\n" +
                                    $"Eksik malzemelerin toplam maliyeti: {eksikMalzemelerMaliyeti:F2} TL\n" +
                                    $"Eksik Malzemeler:\n{eksikMalzemeler}",
                                    "Maliyet Bilgisi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Tarifinizin maliyeti: {toplamMaliyet:F2} TL", "Maliyet Bilgisi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            string aramaMetni = textBox1.Text.ToLower();

            string query = "SELECT * FROM dbo.\"Tariflers\" WHERE LOWER(\"TarifAdi\") LIKE @AramaMetni OR LOWER(\"Talimatlar\") LIKE @AramaMetni";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, connection))
                {
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@AramaMetni", "%" + aramaMetni + "%");

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

                            using (NpgsqlCommand malzemeCommanda = new NpgsqlCommand(malzemeQuery, connection))
                            {
                                malzemeCommanda.Parameters.AddWithValue("@TarifAdi", tarifAdi);
                                using (NpgsqlDataReader malzemeReader = malzemeCommanda.ExecuteReader())
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
            }
        }
    }
}




