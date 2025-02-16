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
    public partial class GüncelleForm : Form
    {
        private Tarifler_control parentControl1;
        private string connectionString = "Server=localhost;Port=5432;Database=mydatabase;Username=myuser;Password=mypassword";

        public GüncelleForm(Tarifler_control parent)
        {
            InitializeComponent();
            parentControl1 = parent;
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
                    // Duplicate kontrolü
                    string duplicateQuery = "SELECT COUNT(*) FROM dbo.\"Tariflers\" WHERE \"TarifAdi\" = @TarifAdi AND \"TarifAdi\" != @EskiTarifAdi";
                    using (var duplicateCommand = new NpgsqlCommand(duplicateQuery, connection))
                    {
                        duplicateCommand.Parameters.AddWithValue("@TarifAdi", textBox1.Text);
                        duplicateCommand.Parameters.AddWithValue("@EskiTarifAdi", parentControl1.dataGridView1.SelectedRows[0].Cells["TarifAdi"].Value.ToString());
                        long count = (long)duplicateCommand.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Bu tarif adı zaten mevcut.", "Duplicate Tarif", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    string updateQuery = "UPDATE dbo.\"Tariflers\" SET \"TarifAdi\" = @TarifAdi, \"Kategori\" = @Kategori, " +
                                         "\"HazirlamaSuresi\" = @HazirlamaSuresi, \"Talimatlar\" = @Talimatlar WHERE \"TarifAdi\" = @EskiTarifAdi"; using (var command = new NpgsqlCommand(updateQuery, connection))
                        {
                            command.Parameters.AddWithValue("@TarifAdi", textBox1.Text);
                            command.Parameters.AddWithValue("@Kategori", comboBox1.SelectedItem.ToString());
                            command.Parameters.AddWithValue("@HazirlamaSuresi", int.Parse(textBox2.Text)); // Sayıya dönüştür
                            command.Parameters.AddWithValue("@Talimatlar", textBox3.Text);
                            command.Parameters.AddWithValue("@EskiTarifAdi", parentControl1.dataGridView1.SelectedRows[0].Cells["TarifAdi"].Value.ToString());
                            command.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Tarif başarıyla güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
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
