using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entity.Exceptions
{
    public class NullObjectException : Exception
    {
        protected NullObjectException(string Message) : base(Message)
        {

        }
    }
}
