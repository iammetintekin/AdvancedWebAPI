using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entity.Exceptions
{
    public class OutOfRangeException : BadRequestException
    {
        public OutOfRangeException(string ColumnName) : base($"Invalid values for : {ColumnName}")
        {
        }
    }
}
