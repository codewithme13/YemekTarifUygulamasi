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
    public partial class Giris_kontrol : UserControl
    {
        private List<Image> images = new List<Image>(); // Resimlerin listesini tutacağız
        private int currentIndex = 0; // Şu anki resim indeksini tutuyor
        private string[] aciklamalar;


        public Giris_kontrol()
        {
            InitializeComponent();
            LoadImages(); // Resimleri yükle
            ShowImage(currentIndex); // İlk resmi göster
        }
        private void LoadImages()
        {
            // Resimlerin bulunduğu klasörün tam yolu
            string basePath = @"C:\Users\umut\source\repos\YemekTarifUgulaması\YemekTarifUgulaması\Pictures";

            // Resim dosyaları ve açıklamaları
            string[] resimDosyalari = {
                "Kebap.jpg",
                "Mantı.jpg",
                "Pilav.jpg",
                "Aşure.jpg",
                "Baklava.jpg",
                "Fırında Makarna.jpg",
                "İskender.jpg",
                "Karnıyarık.jpg",
                "Kazandibi.jpg",
                "Köfte.jpg",
                "Kuzu Tandır.jpg",
                "Künefe.jpg",
                "Lahmacun.jpg",
                "Revani.jpg",
                "Sütlaç.jpg",
                "Tavuk Sote.jpg"
            };

            // Her resim için açıklama
            aciklamalar = new string[] {
                "Kebap: Geleneksel Türk kebabı.",
                "Mantı: Türk mutfağına ait mantı yemeği.",
                "Pilav: Yanında kebap ile sunulan pilav.",
                "Aşure: Geleneksel aşure tatlısı.",
                "Baklava: Şerbetli bir tatlı.",
                "Fırında Makarna: Fırında yapılan makarna yemeği.",
                "İskender: Döner ile yapılan bir yemek.",
                "Karnıyarık: Kızartılmış patlıcan ile yapılan bir yemek.",
                "Kazandibi: Sütlaç benzeri bir tatlı.",
                "Köfte: Kıyma ile yapılan köfte.",
                "Kuzu Tandır: Kuzu etinin tandırda pişirilmesi.",
                "Künefe: Kadayıf ile yapılan bir tatlı.",
                "Lahmacun: İnce hamur üzerine kıymalı harç.",
                "Revani: İrmik tatlısı.",
                "Sütlaç: Pirinçle yapılan süt tatlısı.",
                "Tavuk Sote: Tavuk etinin sotelenmesi."
            };

            for (int i = 0; i < resimDosyalari.Length; i++)
            {
                string tamYol = System.IO.Path.Combine(basePath, resimDosyalari[i]);
                if (!System.IO.File.Exists(tamYol))
                {
                    MessageBox.Show($"Resim dosyası bulunamadı: {tamYol}");
                    continue;
                }
                images.Add(Image.FromFile(tamYol)); // Resmi yükle
            }

            // İlk resmi göster ve açıklamayı ayarla
            ShowImage(currentIndex);
        }

        private void ShowImage(int index)
        {
            // Resim ve açıklama dizisinin boyutlarını kontrol et
            if (images.Count > 0 && index >= 0 && index < aciklamalar.Length)
            {
                pictureBox2.Image = images[index]; // Resmi göster
                labelDescription.Text = aciklamalar[index]; // Açıklamayı güncelle
            }
            else
            {
                // Eğer index geçersizse, hata mesajı veya varsayılan değer ayarlayabilirsiniz
                labelDescription.Text = "Açıklama mevcut değil."; // Varsayılan bir açıklama
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Sağ butona tıklandığında
            currentIndex = (currentIndex + 1) % images.Count; // Bir sonraki resme git
            ShowImage(currentIndex); // Resmi göster
        }

        private void button1_Click(object sender, EventArgs e)
        {
            currentIndex = (currentIndex - 1 + images.Count) % images.Count; // Bir önceki resme git
            ShowImage(currentIndex); // Resmi göster
        }

    }
}
