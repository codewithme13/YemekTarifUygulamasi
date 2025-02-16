using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace YemekTarifUgulaması
{
    public partial class Malzemeler_control : UserControl
    {
        private string connectionString = "Server=localhost;Port=5432;Database=mydatabase;Username=myuser;Password=mypassword";

        public Malzemeler_control()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM dbo.\"Malzemelers\" ORDER BY \"MalzemeID\" ASC";

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
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Connection Stringinde hata var :) " + ex.Message);
                }
            }
            textBox1.Clear();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string searchText = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                button1_Click(sender, e); 
                return;
            }

            string query = "SELECT * FROM dbo.\"Malzemelers\" WHERE \"MalzemeAdi\" ILIKE @SearchText ORDER BY \"MalzemeID\" ASC";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    NpgsqlCommand command = new NpgsqlCommand(query, connection);
                    command.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");

                    NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Arama sırasında bir hata oluştu: " + ex.Message);
                }
            }
        }
    }
}
