using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entity.Exceptions
{ 
    /// <summary>
    /// Customer error type. Sealed class blocks the inherits
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class EntityNotFoundException<T> : NotFoundException
        {
            public EntityNotFoundException(int ID) : base($"The {typeof(T)} with ID : {ID} couldn't found in database.")
            {
            }
        }
}
