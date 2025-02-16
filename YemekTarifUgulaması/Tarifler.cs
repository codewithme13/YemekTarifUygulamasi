using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YemekTarifUgulaması
{
    public class Tarifler
    {
        [Key]
        public int TarifID { get; set; }

        public string TarifAdi { get; set; }

        public string Kategori { get; set; }

        public int HazirlamaSuresi { get; set; }

        public string Talimatlar { get; set; }
        public ICollection<Tarif_Malzeme> TarifMalzemeler { get; set; }
    }
}
