using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entity.DTOs.Product
{
    /// <summary>
    /// dtos must be readonly and can't be changed. linq support and reference type. ctor
    /// </summary>
    public record UpdateProductDto: ProductDtoManipulator
    {
        //init : tanımlandığı yerde set edilmeli, sonradan değiştirlmesin engeller. 
        public int Id { get; init; } 
    }
}
