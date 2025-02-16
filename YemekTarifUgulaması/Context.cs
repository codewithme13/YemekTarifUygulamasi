using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YemekTarifUgulaması
{
   public class Context : DbContext
    {
        public Context(): base("Name=Context")
        {

        }
        public DbSet<Tarifler> Tariflers { get; set; }
        public DbSet<Malzemeler> Malzemelers { get; set; }
        public DbSet<Tarif_Malzeme> Tarif_Malzemes { get; set; }
    }
}
