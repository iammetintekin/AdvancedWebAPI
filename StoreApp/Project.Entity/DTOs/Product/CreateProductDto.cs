using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entity.DTOs.Product
{
    //recordlar birbirini kalıtır sadece.
    public record CreateProductDto :ProductDtoManipulator
    {
        // no Id field
    }
}
