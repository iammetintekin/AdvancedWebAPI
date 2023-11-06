using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entity.Exceptions
{
    public sealed class EntityIsNullException<T> : NullObjectException
    {
        public EntityIsNullException() : base($"The object {typeof(T)} is NULL.")
        {
        }
    }
}
