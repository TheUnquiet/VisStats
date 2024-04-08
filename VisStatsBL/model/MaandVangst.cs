using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisStatsBL.model
{
    public class MaandVangst
    {
        public MaandVangst(string soortNaam, double total, int maand, int jaar)
        {
            SoortNaam = soortNaam;
            Total = total;
            Maand = maand;
            Jaar = jaar;
        }

        public string SoortNaam { get; set; }
        public double Total { get; set; }
        public int Maand { get; set; }
        public int Jaar { get; set; }
    }
}
