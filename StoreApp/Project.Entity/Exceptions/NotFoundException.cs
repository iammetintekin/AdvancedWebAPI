using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entity.Exceptions
{
    public abstract class NotFoundException:Exception
    { 
        protected NotFoundException(string Message) :base(Message)
        {
            
        }
    }
}
