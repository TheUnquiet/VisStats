using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisStatsBL.exceptions;

namespace VisStatsBL.model
{
    public class Vissoort
    {
        public int? Id;
        private string naam;

        public string Naam
        {
            get { return naam; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new domeinException("Visoort_naam");
                naam = value;
            }
        }

        public Vissoort(string naam)
        {
            this.naam = naam;
        }

        public Vissoort(int? id, string naam)
        {
            Id = id;
            this.naam = naam;
        }
        public override string ToString()
        {
            return naam;
        }
    }
}
