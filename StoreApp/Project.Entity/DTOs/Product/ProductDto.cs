using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entity.DTOs.Product
{
    //propları var ise serialize edilebilir.
    public record ProductDto: ProductDtoManipulator
    {
        public int Id { get; init; }
        
    }
}
