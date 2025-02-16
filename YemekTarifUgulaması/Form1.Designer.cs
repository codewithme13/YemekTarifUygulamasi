
namespace YemekTarifUgulaması
{
    partial class Form1
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.AnaPanel = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Tarif_Önerisi = new System.Windows.Forms.Button();
            this.panelslide = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Arama_ve_Filitreleme = new System.Windows.Forms.Button();
            this.Malzeme_Yönetimi = new System.Windows.Forms.Button();
            this.Tarif_Ekleme = new System.Windows.Forms.Button();
            this.TARİFLER = new System.Windows.Forms.Button();
            this.GİRİŞ = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.arama_Filtreleme1 = new YemekTarifUgulaması.Arama_Filtreleme();
            this.tarif_Yönetimi1 = new YemekTarifUgulaması.Tarif_Yönetimi();
            this.tarif_Ekle_control1 = new YemekTarifUgulaması.Tarif_Ekle_control();
            this.malzemeler_control1 = new YemekTarifUgulaması.Malzemeler_control();
            this.tarifler_control1 = new YemekTarifUgulaması.Tarifler_control();
            this.giris_kontrol3 = new YemekTarifUgulaması.Giris_kontrol();
            this.giris_kontrol2 = new YemekTarifUgulaması.Giris_kontrol();
            this.giris_kontrol1 = new YemekTarifUgulaması.Giris_kontrol();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.CornflowerBlue;
            this.panel1.Controls.Add(this.AnaPanel);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.Tarif_Önerisi);
            this.panel1.Controls.Add(this.panelslide);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.Arama_ve_Filitreleme);
            this.panel1.Controls.Add(this.Malzeme_Yönetimi);
            this.panel1.Controls.Add(this.Tarif_Ekleme);
            this.panel1.Controls.Add(this.TARİFLER);
            this.panel1.Controls.Add(this.GİRİŞ);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(197, 677);
            this.panel1.TabIndex = 0;
            // 
            // AnaPanel
            // 
            this.AnaPanel.Location = new System.Drawing.Point(195, 51);
            this.AnaPanel.Name = "AnaPanel";
            this.AnaPanel.Size = new System.Drawing.Size(1025, 623);
            this.AnaPanel.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(133, 51);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(45, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MV Boli", 21.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 39);
            this.label3.TabIndex = 10;
            this.label3.Text = "Tarif";
            // 
            // Tarif_Önerisi
            // 
            this.Tarif_Önerisi.BackColor = System.Drawing.Color.LightSkyBlue;
            this.Tarif_Önerisi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Tarif_Önerisi.Image = ((System.Drawing.Image)(resources.GetObject("Tarif_Önerisi.Image")));
            this.Tarif_Önerisi.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Tarif_Önerisi.Location = new System.Drawing.Point(22, 521);
            this.Tarif_Önerisi.Name = "Tarif_Önerisi";
            this.Tarif_Önerisi.Size = new System.Drawing.Size(175, 53);
            this.Tarif_Önerisi.TabIndex = 9;
            this.Tarif_Önerisi.Text = "     TARİF ÖNERİSİ";
            this.Tarif_Önerisi.UseVisualStyleBackColor = false;
            this.Tarif_Önerisi.Click += new System.EventHandler(this.Tarif_Önerisi_Click_1);
            // 
            // panelslide
            // 
            this.panelslide.BackColor = System.Drawing.Color.Tomato;
            this.panelslide.Location = new System.Drawing.Point(3, 172);
            this.panelslide.Name = "panelslide";
            this.panelslide.Size = new System.Drawing.Size(18, 53);
            this.panelslide.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MV Boli", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(16, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(182, 31);
            this.label2.TabIndex = 8;
            this.label2.Text = "UYGULAMASI";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MV Boli", 21.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(33, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 39);
            this.label1.TabIndex = 0;
            this.label1.Text = "Yemek";
            // 
            // Arama_ve_Filitreleme
            // 
            this.Arama_ve_Filitreleme.BackColor = System.Drawing.Color.LightSkyBlue;
            this.Arama_ve_Filitreleme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Arama_ve_Filitreleme.Image = ((System.Drawing.Image)(resources.GetObject("Arama_ve_Filitreleme.Image")));
            this.Arama_ve_Filitreleme.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Arama_ve_Filitreleme.Location = new System.Drawing.Point(22, 612);
            this.Arama_ve_Filitreleme.Name = "Arama_ve_Filitreleme";
            this.Arama_ve_Filitreleme.Size = new System.Drawing.Size(175, 53);
            this.Arama_ve_Filitreleme.TabIndex = 7;
            this.Arama_ve_Filitreleme.Text = "           ARAMA-FİLİTRELEME";
            this.Arama_ve_Filitreleme.UseVisualStyleBackColor = false;
            this.Arama_ve_Filitreleme.Click += new System.EventHandler(this.Arama_ve_Filitreleme_Click_1);
            // 
            // Malzeme_Yönetimi
            // 
            this.Malzeme_Yönetimi.BackColor = System.Drawing.Color.LightSkyBlue;
            this.Malzeme_Yönetimi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Malzeme_Yönetimi.Image = ((System.Drawing.Image)(resources.GetObject("Malzeme_Yönetimi.Image")));
            this.Malzeme_Yönetimi.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Malzeme_Yönetimi.Location = new System.Drawing.Point(22, 432);
            this.Malzeme_Yönetimi.Name = "Malzeme_Yönetimi";
            this.Malzeme_Yönetimi.Size = new System.Drawing.Size(175, 53);
            this.Malzeme_Yönetimi.TabIndex = 6;
            this.Malzeme_Yönetimi.Text = "         MALZEME YÖNETİMİ";
            this.Malzeme_Yönetimi.UseVisualStyleBackColor = false;
            this.Malzeme_Yönetimi.Click += new System.EventHandler(this.Malzeme_Yönetimi_Click_1);
            // 
            // Tarif_Ekleme
            // 
            this.Tarif_Ekleme.BackColor = System.Drawing.Color.LightSkyBlue;
            this.Tarif_Ekleme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Tarif_Ekleme.Image = ((System.Drawing.Image)(resources.GetObject("Tarif_Ekleme.Image")));
            this.Tarif_Ekleme.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Tarif_Ekleme.Location = new System.Drawing.Point(22, 344);
            this.Tarif_Ekleme.Name = "Tarif_Ekleme";
            this.Tarif_Ekleme.Size = new System.Drawing.Size(175, 53);
            this.Tarif_Ekleme.TabIndex = 2;
            this.Tarif_Ekleme.Text = "       YENİ TARİF EKLE";
            this.Tarif_Ekleme.UseVisualStyleBackColor = false;
            this.Tarif_Ekleme.Click += new System.EventHandler(this.Tarif_Ekleme_Click);
            // 
            // TARİFLER
            // 
            this.TARİFLER.BackColor = System.Drawing.Color.LightSkyBlue;
            this.TARİFLER.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TARİFLER.Image = ((System.Drawing.Image)(resources.GetObject("TARİFLER.Image")));
            this.TARİFLER.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.TARİFLER.Location = new System.Drawing.Point(22, 258);
            this.TARİFLER.Name = "TARİFLER";
            this.TARİFLER.Size = new System.Drawing.Size(175, 53);
            this.TARİFLER.TabIndex = 1;
            this.TARİFLER.Text = "TARİFLER";
            this.TARİFLER.UseVisualStyleBackColor = false;
            this.TARİFLER.Click += new System.EventHandler(this.Tarif_Listesi_Click);
            // 
            // GİRİŞ
            // 
            this.GİRİŞ.BackColor = System.Drawing.Color.LightSkyBlue;
            this.GİRİŞ.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GİRİŞ.Image = ((System.Drawing.Image)(resources.GetObject("GİRİŞ.Image")));
            this.GİRİŞ.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.GİRİŞ.Location = new System.Drawing.Point(22, 172);
            this.GİRİŞ.Name = "GİRİŞ";
            this.GİRİŞ.Size = new System.Drawing.Size(175, 53);
            this.GİRİŞ.TabIndex = 0;
            this.GİRİŞ.Text = "GİRİŞ";
            this.GİRİŞ.UseVisualStyleBackColor = false;
            this.GİRİŞ.Click += new System.EventHandler(this.GİRİŞ_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.LightSkyBlue;
            this.panel2.Controls.Add(this.button1);
            this.panel2.Location = new System.Drawing.Point(195, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1023, 38);
            this.panel2.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Tomato;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(981, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(42, 38);
            this.button1.TabIndex = 0;
            this.button1.Text = "X";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.arama_Filtreleme1);
            this.panel3.Controls.Add(this.tarif_Yönetimi1);
            this.panel3.Controls.Add(this.tarif_Ekle_control1);
            this.panel3.Controls.Add(this.malzemeler_control1);
            this.panel3.Controls.Add(this.tarifler_control1);
            this.panel3.Location = new System.Drawing.Point(197, 38);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1023, 639);
            this.panel3.TabIndex = 2;
            // 
            // arama_Filtreleme1
            // 
            this.arama_Filtreleme1.BackColor = System.Drawing.Color.White;
            this.arama_Filtreleme1.ForeColor = System.Drawing.Color.Black;
            this.arama_Filtreleme1.Location = new System.Drawing.Point(-2, 0);
            this.arama_Filtreleme1.Name = "arama_Filtreleme1";
            this.arama_Filtreleme1.Size = new System.Drawing.Size(1023, 639);
            this.arama_Filtreleme1.TabIndex = 6;
            // 
            // tarif_Yönetimi1
            // 
            this.tarif_Yönetimi1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tarif_Yönetimi1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.tarif_Yönetimi1.Location = new System.Drawing.Point(0, 0);
            this.tarif_Yönetimi1.Margin = new System.Windows.Forms.Padding(4);
            this.tarif_Yönetimi1.Name = "tarif_Yönetimi1";
            this.tarif_Yönetimi1.Size = new System.Drawing.Size(1023, 639);
            this.tarif_Yönetimi1.TabIndex = 5;
            // 
            // tarif_Ekle_control1
            // 
            this.tarif_Ekle_control1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tarif_Ekle_control1.Location = new System.Drawing.Point(2, 0);
            this.tarif_Ekle_control1.Name = "tarif_Ekle_control1";
            this.tarif_Ekle_control1.Size = new System.Drawing.Size(1022, 639);
            this.tarif_Ekle_control1.TabIndex = 3;
            // 
            // malzemeler_control1
            // 
            this.malzemeler_control1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.malzemeler_control1.Location = new System.Drawing.Point(-2, 0);
            this.malzemeler_control1.Name = "malzemeler_control1";
            this.malzemeler_control1.Size = new System.Drawing.Size(1023, 639);
            this.malzemeler_control1.TabIndex = 2;
            // 
            // tarifler_control1
            // 
            this.tarifler_control1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.tarifler_control1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tarifler_control1.Location = new System.Drawing.Point(0, 0);
            this.tarifler_control1.Name = "tarifler_control1";
            this.tarifler_control1.Size = new System.Drawing.Size(1023, 639);
            this.tarifler_control1.TabIndex = 0;
            // 
            // giris_kontrol3
            // 
            this.giris_kontrol3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.giris_kontrol3.Location = new System.Drawing.Point(-2, 0);
            this.giris_kontrol3.Name = "giris_kontrol3";
            this.giris_kontrol3.Size = new System.Drawing.Size(1023, 639);
            this.giris_kontrol3.TabIndex = 4;
            // 
            // giris_kontrol2
            // 
            this.giris_kontrol2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(140)))), ((int)(((byte)(0)))));
            this.giris_kontrol2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.giris_kontrol2.Location = new System.Drawing.Point(0, -639);
            this.giris_kontrol2.Name = "giris_kontrol2";
            this.giris_kontrol2.Size = new System.Drawing.Size(1023, 639);
            this.giris_kontrol2.TabIndex = 1;
            // 
            // giris_kontrol1
            // 
            this.giris_kontrol1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(140)))), ((int)(((byte)(0)))));
            this.giris_kontrol1.Location = new System.Drawing.Point(0, -13);
            this.giris_kontrol1.Name = "giris_kontrol1";
            this.giris_kontrol1.Size = new System.Drawing.Size(1025, 644);
            this.giris_kontrol1.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1220, 677);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Yemek Tarif Uygulaması";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Arama_ve_Filitreleme;
        private System.Windows.Forms.Button Malzeme_Yönetimi;
        private System.Windows.Forms.Button Tarif_Ekleme;
        private System.Windows.Forms.Button TARİFLER;
        private System.Windows.Forms.Button GİRİŞ;
        private System.Windows.Forms.FlowLayoutPanel panelslide;
        private System.Windows.Forms.Button Tarif_Önerisi;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel AnaPanel;
        private System.Windows.Forms.Panel panel2;
        private Giris_kontrol giris_kontrol1;
        private System.Windows.Forms.Panel panel3;
        private Giris_kontrol giris_kontrol2;
        private Tarifler_control tarifler_control1;
        private Malzemeler_control malzemeler_control1;
        private Tarif_Ekle_control tarif_Ekle_control1;
        private Giris_kontrol giris_kontrol3;
        private Tarif_Yönetimi tarif_Yönetimi1;
        private Arama_Filtreleme arama_Filtreleme1;
        private System.Windows.Forms.Button button1;
    }
}

