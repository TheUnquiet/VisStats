using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisStatsBL.exceptions
{
    public class domeinException : Exception
    {
        public domeinException(string? message) : base(message)
        {
        }

        public domeinException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
