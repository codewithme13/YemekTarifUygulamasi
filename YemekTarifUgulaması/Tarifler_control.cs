using System;
using System.Data;
using Npgsql;
using System.Windows.Forms;
using System.Drawing;


namespace YemekTarifUgulaması
{
    public partial class Tarifler_control : UserControl
    {
        public string connectionString = "Server=localhost;Port=5432;Database=mydatabase;Username=myuser;Password=mypassword";

        public Tarifler_control()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            comboBox1.SelectedIndex = -1;
            string query =
                "SELECT t.\"TarifAdi\", t.\"HazirlamaSuresi\", " +
                "COALESCE(SUM(tm.\"MalzemeMiktar\" * m.\"BirimFiyat\"), 0) AS \"Maliyet\" " +
                "FROM dbo.\"Tariflers\" t " +
                "LEFT JOIN dbo.\"Tarif_Malzeme\" tm ON t.\"TarifID\" = tm.\"TarifID\" " +
                "LEFT JOIN dbo.\"Malzemelers\" m ON tm.\"MalzemeID\" = m.\"MalzemeID\" " +
                "GROUP BY t.\"TarifID\", t.\"TarifAdi\", t.\"HazirlamaSuresi\" " +
                "ORDER BY t.\"TarifID\" ASC";

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
                    MessageBox.Show("Veritabanına bağlanırken bir hata oluştu: " + ex.Message);
                }
            }
        }






        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen güncellemek istediğiniz bir tarif seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            GüncelleForm güncelleForm = new GüncelleForm(this);

            string tarifAdi = dataGridView1.SelectedRows[0].Cells["TarifAdi"].Value.ToString();

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT \"Kategori\", \"HazirlamaSuresi\", \"Talimatlar\" FROM dbo.\"Tariflers\" WHERE \"TarifAdi\" = @TarifAdi";
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TarifAdi", tarifAdi);
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                güncelleForm.textBox1.Text = tarifAdi;
                                güncelleForm.textBox2.Text = reader["HazirlamaSuresi"].ToString();
                                güncelleForm.textBox3.Text = reader["Talimatlar"].ToString();
                                güncelleForm.comboBox1.SelectedItem = reader["Kategori"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Veri çekme işlemi sırasında bir hata oluştu: " + ex.Message);
                }
            }

            güncelleForm.ShowDialog();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen silmek istediğiniz bir tarif seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Seçilen tarifi silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                string tarifAdi = dataGridView1.SelectedRows[0].Cells["TarifAdi"].Value.ToString();
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        string deleteTarifMalzemeQuery = "DELETE FROM dbo.\"Tarif_Malzeme\" WHERE \"TarifID\" = (SELECT \"TarifID\" FROM dbo.\"Tariflers\" WHERE \"TarifAdi\" = @TarifAdi)";
                        using (NpgsqlCommand command = new NpgsqlCommand(deleteTarifMalzemeQuery, connection))
                        {
                            command.Parameters.AddWithValue("@TarifAdi", tarifAdi);
                            command.ExecuteNonQuery();
                        }

                        string deleteTarifQuery = "DELETE FROM dbo.\"Tariflers\" WHERE \"TarifAdi\" = @TarifAdi";
                        using (NpgsqlCommand command = new NpgsqlCommand(deleteTarifQuery, connection))
                        {
                            command.Parameters.AddWithValue("@TarifAdi", tarifAdi);
                            command.ExecuteNonQuery();
                        }

                        MessageBox.Show("Tarif başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Silme işlemi sırasında bir hata oluştu: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Silme işlemi iptal edildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }



        private void textBox1_TextChanged(object sender, EventArgs e)
        {
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
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen bir sıralama seçeneği seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string secim = comboBox1.SelectedItem.ToString();
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
                default:
                    MessageBox.Show("Geçersiz sıralama seçeneği.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

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





        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                string tarifAdi = row.Cells["TarifAdi"].Value.ToString();

                string query = "SELECT * FROM dbo.\"Tariflers\" WHERE \"TarifAdi\" = @TarifAdi";

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        NpgsqlCommand command = new NpgsqlCommand(query, connection);
                        command.Parameters.AddWithValue("@TarifAdi", tarifAdi);

                        NpgsqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            string kategori = reader["Kategori"].ToString();
                            int hazirlamaSuresi = Convert.ToInt32(reader["HazirlamaSuresi"]);
                            string talimatlar = reader["Talimatlar"].ToString();

                            MessageBox.Show($"Tarif Adı: {tarifAdi}\nKategori: {kategori}\nHazırlama Süresi: {hazirlamaSuresi} dakika\nTalimatlar: {talimatlar}", "Tarif Detayları", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Detayları çekerken bir hata oluştu: " + ex.Message);
                    }
                }
            }
        }
    }
}
