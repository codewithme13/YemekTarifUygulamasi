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

    public partial class Tarif_Ekle_control : UserControl
    {
        private string connectionString = "Server=localhost;Port=5432;Database=mydatabase;Username=myuser;Password=mypassword";

        public Tarif_Ekle_control()
        {
            InitializeComponent();
            LoadMalzemeler();
        }

        public void LoadMalzemeler()
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

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null)
            {
                string EklenecekMalzeme = comboBox2.SelectedItem.ToString();
                listBox1.Items.Add(EklenecekMalzeme);
            }
        }
        private void OnMalzemeChanged()
        {
            LoadMalzemeler();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string EklenecekMalzeme = comboBox2.SelectedItem?.ToString();
            string EklenecekMiktar = textBox4.Text;
            string EklenecekMalzemeBirim = comboBox3.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(EklenecekMalzeme) || string.IsNullOrEmpty(EklenecekMalzemeBirim) || string.IsNullOrEmpty(EklenecekMiktar))
            {
                MessageBox.Show("Lütfen bir malzeme seçin ve eklemek için bir değer girin.");
                return;
            }

            if (!double.TryParse(EklenecekMiktar, out double miktar))
            {
                MessageBox.Show("Lütfen geçerli bir miktar girin.");
                return;
            }
            
            string malzemeToAdd = $"{EklenecekMalzeme} - {EklenecekMiktar} - {EklenecekMalzemeBirim} ";
            listBox1.Items.Add(malzemeToAdd);

        }





        private void button2_Click(object sender, EventArgs e)
        {
            YeniMalzemeForm yeniMalzemeForm = new YeniMalzemeForm(this);
            yeniMalzemeForm.ShowDialog();
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.' && e.KeyChar != ','))
            {
                MessageBox.Show("Lütfen sayısal bir değer giriniz.", "Geçersiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                e.Handled = true;
            }
            if ((e.KeyChar == '.' || e.KeyChar == ',') && (sender as TextBox).Text.IndexOfAny(new char[] { '.', ',' }) > -1)
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.' && e.KeyChar != ','))
            {
                MessageBox.Show("Lütfen sayısal bir değer giriniz.", "Geçersiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                e.Handled = true;
            }
            if ((e.KeyChar == '.' || e.KeyChar == ',') && (sender as TextBox).Text.IndexOfAny(new char[] { '.', ',' }) > -1)
            {
                e.Handled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                comboBox1.SelectedItem == null ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                comboBox3.SelectedItem == null)
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Duplicate kontrolü
                    string duplicateQuery = "SELECT COUNT(*) FROM dbo.\"Tariflers\" WHERE \"TarifAdi\" = @TarifAdi";
                    using (var duplicateCommand = new NpgsqlCommand(duplicateQuery, connection))
                    {
                        duplicateCommand.Parameters.AddWithValue("@TarifAdi", textBox1.Text);
                        long count = (long)duplicateCommand.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Bu tarif zaten mevcut.", "Duplicate Tarif", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    string query = "INSERT INTO dbo.\"Tariflers\" (\"TarifAdi\", \"Kategori\", \"HazirlamaSuresi\", \"Talimatlar\") VALUES (@TarifAdi, @Kategori, @HazilramaSuresi, @Talimatlar) RETURNING \"TarifID\"";
                    int tarifId;

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TarifAdi", textBox1.Text);
                        command.Parameters.AddWithValue("@Kategori", comboBox1.SelectedItem.ToString());
                        command.Parameters.AddWithValue("@HazilramaSuresi", int.Parse(textBox2.Text));
                        command.Parameters.AddWithValue("@Talimatlar", textBox3.Text);

                        tarifId = (int)command.ExecuteScalar();
                    }

                    double TotalMaliyet = 0; // Maliyeti buraya taşıdık

                    
                    foreach (var item in listBox1.Items)
                    {
                        string malzemeBilgisi = item.ToString();
                        string[] bilgiler = malzemeBilgisi.Split('-');

                        string malzemeAdi = bilgiler[0].Trim();
                        double kullanilanMiktar = double.Parse(bilgiler[1].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                        string girilenBirim = bilgiler[2].Trim(); 

                        string selectQuery = "SELECT \"MalzemeBirim\", \"ToplamMiktar\", \"BirimFiyat\" FROM dbo.\"Malzemelers\" WHERE \"MalzemeAdi\" = @MalzemeAdi";
                        string malzemeBirim = null;
                        double toplamMiktar = 0;
                        double birimFiyat = 0;

                        using (var selectCommand = new NpgsqlCommand(selectQuery, connection))
                        {
                            selectCommand.Parameters.AddWithValue("@MalzemeAdi", malzemeAdi);

                            using (var reader = selectCommand.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    malzemeBirim = reader.GetString(0);
                                    toplamMiktar = reader.GetDouble(1);
                                    birimFiyat = reader.GetDouble(2);
                                }
                                else
                                {
                                    MessageBox.Show("Malzeme bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    continue;
                                }
                            }
                        }

                       
                        double TarifMiktarı = kullanilanMiktar;
                        double Maliyet = 0.00;

                        if (malzemeBirim == "kg" && girilenBirim == "gram")
                        {
                            TarifMiktarı /= 1000;
                        }
                        else if (malzemeBirim == "litre" && girilenBirim == "mililitre")
                        {
                            TarifMiktarı /= 1000;
                        }

                        toplamMiktar -= TarifMiktarı; 

                        Maliyet = TarifMiktarı * birimFiyat;

                        
                        string insertMalzemeQuery = "INSERT INTO dbo.\"Tarif_Malzeme\" (\"TarifID\", \"MalzemeID\", \"MalzemeMiktar\") VALUES (@TarifID, (SELECT \"MalzemeID\" FROM dbo.\"Malzemelers\" WHERE \"MalzemeAdi\" = @MalzemeAdi), @MalzemeMiktar)";
                        using (var insertCommand = new NpgsqlCommand(insertMalzemeQuery, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@TarifID", tarifId);
                            insertCommand.Parameters.AddWithValue("@MalzemeAdi", malzemeAdi);
                            insertCommand.Parameters.AddWithValue("@MalzemeMiktar", TarifMiktarı);
                            insertCommand.ExecuteNonQuery();
                        }

                        TotalMaliyet += Maliyet;
                    }

                    MessageBox.Show($"Maliyet: {TotalMaliyet:F2} TL", "Maliyet Bilgisi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show("Tarif başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    textBox4.Clear();
                    listBox1.Items.Clear();
                    comboBox1.SelectedIndex = -1;
                    comboBox3.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
