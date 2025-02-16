using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace YemekTarifUgulaması
{
    public partial class Form1 : Form
    {
        Giris_kontrol Giris_Kontrol = new Giris_kontrol();
        Tarifler_control Tarifler_Control = new Tarifler_control();
        Malzemeler_control Malzemeler_Control = new Malzemeler_control();
        Tarif_Ekle_control Tarif_Ekle_Control = new Tarif_Ekle_control();
        Tarif_Yönetimi Tarif_Yönetimi = new Tarif_Yönetimi();
        Arama_Filtreleme Arama_Filtreleme = new Arama_Filtreleme();

        public Form1()
        {
            InitializeComponent();
            panel3.Controls.Clear();
            panel3.Controls.Add(Giris_Kontrol);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void movesidepanel (Button btn)
        {
            
            panelslide.Top = btn.Top;
            panelslide.Height = btn.Height;
        }



        private void Tarif_Listesi_Click(object sender, EventArgs e)
        {
            movesidepanel(TARİFLER);
            panel3.Controls.Clear();
            panel3.Controls.Add(Tarifler_Control);

        }

        private void Tarif_Ekleme_Click(object sender, EventArgs e)
        {
            movesidepanel(Tarif_Ekleme);
            panel3.Controls.Clear();
            panel3.Controls.Add(Tarif_Ekle_Control);


        }

        private void Arama_ve_Filitreleme_Click_1(object sender, EventArgs e)
        {
            movesidepanel(Arama_ve_Filitreleme);
                panel3.Controls.Clear();
                panel3.Controls.Add(Arama_Filtreleme); 

        }

        private void Tarif_Önerisi_Click_1(object sender, EventArgs e)
        {
            movesidepanel(Tarif_Önerisi);
            panel3.Controls.Clear();
            panel3.Controls.Add(Tarif_Yönetimi);
        }

        private void Malzeme_Yönetimi_Click_1(object sender, EventArgs e)
        {
            movesidepanel(Malzeme_Yönetimi);
            panel3.Controls.Clear();
            panel3.Controls.Add(Malzemeler_Control);
        }


        private void GİRİŞ_Click(object sender, EventArgs e)
        {
                movesidepanel(GİRİŞ);
                panel3.Controls.Clear();
                panel3.Controls.Add(Giris_Kontrol); 

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();

        }
    }
}
