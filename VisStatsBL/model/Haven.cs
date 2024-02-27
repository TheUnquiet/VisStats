using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisStatsBL.exceptions;

namespace VisStatsBL.model
{
    public class Haven
    {
        public int? Id;
        private string naam;

        public string Naam
        {
            get { return naam; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new domeinException("Haven_naam");
                naam = value;
            }
        }

        public Haven(string naam)
        {
            this.naam = naam;
        }

        public Haven(int? id, string naam)
        {
            Id = id;
            this.naam = Naam;
        }
    }
}
