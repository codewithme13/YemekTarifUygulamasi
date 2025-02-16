using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YemekTarifUgulaması
{
    public class Malzemeler
    {
        [Key]
        public int MalzemeID { get; set; } 

        public string MalzemeAdi { get; set; }  

        public decimal ToplamMiktar { get; set; }  

        public string MalzemeBirim { get; set; }

        public decimal BirimFiyat { get; set; }

        public ICollection<Tarif_Malzeme> TarifMalzemeler { get; set; }
    }
}
