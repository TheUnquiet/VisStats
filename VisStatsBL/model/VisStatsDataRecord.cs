using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisStatsBL.exceptions;

namespace VisStatsBL.model
{
    public class VisStatsDataRecord
    {
        private int jaar;
        public int Jaar
        {
            get { return jaar; }
            set
            {
                if ((value < 2000) || (value > 2100)) throw new domeinException("jaar niet correct"); jaar = value;
            }
        }

        private int maand;
        public int Maand { 
            get { return maand; }
            set
            {
                if ((value < 1) || (value > 12)) throw new domeinException("maand niet correcr"); maand = value;
            }
        }

        private double gewicht;
        public double Gewicht
        {
            get { return gewicht; }
            set
            {
                if (value < 0) throw new domeinException("gewicht niet correcr"); gewicht = value;
            }
        }

        private double waarde;
        public double Waarde
        {
            get { return waarde; }
            set
            {
                if (value < 0) throw new domeinException("waarde niet correcr"); waarde = value;
            }
        }

        private Haven haven;
        public Haven Haven { 
            get { return haven; }
            set
            {
                if (value == null) throw new domeinException("haven is null"); haven = value;
            }
        }
        private Vissoort vissoort;

        public VisStatsDataRecord(int jaar, int maand, double gewicht, double waarde, Haven haven, Vissoort vissoort)
        {
            Jaar = jaar;
            Maand = maand;
            Gewicht = gewicht;
            Waarde = waarde;
            Haven = haven;
            Vissoort = vissoort;
        }

        public Vissoort Vissoort
        {
            get { return vissoort; }
            set
            {
                if (value == null) throw new domeinException("vissoort is null"); vissoort = value;
            }
        }
    }
}
