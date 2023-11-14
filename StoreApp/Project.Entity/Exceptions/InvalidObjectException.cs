using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entity.Exceptions
{
    public class InvalidObjectException<T> : NullObjectException
    {
        public InvalidObjectException() : base($"The object {typeof(T)} is INVALID.")
        {
        }
        public InvalidObjectException(string Description) : base($"The object {typeof(T)} is INVALID. \n" + Description)
        {

        }
    }
}
