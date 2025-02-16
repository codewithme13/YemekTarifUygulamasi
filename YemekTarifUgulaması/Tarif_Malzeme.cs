using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YemekTarifUgulaması
{
    public class Tarif_Malzeme
    {
        [Key]
        public int ID { get; set; } 

        public int TarifID { get; set; }

        [ForeignKey("TarifID")]
        public virtual Tarifler Tarif { get; set; } 

        public int MalzemeID { get; set; } 

        [ForeignKey("MalzemeID")]
        public virtual Malzemeler Malzeme { get; set; } 

        public float MalzemeMiktar { get; set; }
    }
}
