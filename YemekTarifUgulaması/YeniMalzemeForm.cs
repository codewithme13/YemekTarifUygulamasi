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
    public partial class YeniMalzemeForm : Form
    {
        private Tarif_Ekle_control parentControl; // Ana forma referans
        private string connectionString = "Server=localhost;Port=5432;Database=mydatabase;Username=myuser;Password=mypassword";

        public YeniMalzemeForm(Tarif_Ekle_control parent)
        {
            InitializeComponent();
            parentControl = parent;
        }


        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.' && e.KeyChar != ','))
            {
                MessageBox.Show("Lütfen sayısal bir miktar giriniz.", "Geçersiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                e.Handled = true;
            }
            if ((e.KeyChar == '.' || e.KeyChar == ',') && (sender as TextBox).Text.IndexOfAny(new char[] { '.', ',' }) > -1)
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) || comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string checkQuery = "SELECT COUNT(*) FROM dbo.\"Malzemelers\" WHERE \"MalzemeAdi\" = @MalzemeAdi";
                    using (var checkCommand = new NpgsqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@MalzemeAdi", NpgsqlTypes.NpgsqlDbType.Varchar, textBox1.Text);

                        object result = checkCommand.ExecuteScalar();
                        int count = result != null ? Convert.ToInt32(result) : 0;

                        if (count > 0)
                        {
                            MessageBox.Show("Bu malzeme zaten kayıtlı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    string query = "INSERT INTO dbo.\"Malzemelers\" (\"MalzemeAdi\", \"ToplamMiktar\", \"MalzemeBirim\", \"BirimFiyat\") VALUES (@MalzemeAdi, @ToplamMiktar, @MalzemeBirim, @BirimFiyat)";
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MalzemeAdi", textBox1.Text);
                        command.Parameters.AddWithValue("@ToplamMiktar", int.Parse(textBox2.Text));
                        command.Parameters.AddWithValue("@MalzemeBirim", comboBox1.SelectedItem.ToString());
                        command.Parameters.AddWithValue("@BirimFiyat", decimal.Parse(textBox3.Text)); 

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Malzeme başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
                parentControl.LoadMalzemeler();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
