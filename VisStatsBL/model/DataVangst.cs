using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisStatsBL.model
{
    public class DataVangst
    {
        // data transfer object
        public string SoortNaam { get; set; }
        public double Totaal { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public double Gemiddelde { get; set; }

        public DataVangst(string soortNaam, double totaal, double min, double max, double gem)
        {
            SoortNaam = soortNaam;
            Totaal = totaal;
            Min = min;
            Max = max;
            Gemiddelde = gem;
        }
    }
}
