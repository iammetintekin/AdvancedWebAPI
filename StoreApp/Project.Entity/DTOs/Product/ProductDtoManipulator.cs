using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entity.DTOs.Product
{
    public abstract record ProductDtoManipulator
    {
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Name { get; init; }
        [Required]
        [Range(1,1000)]
        public decimal Price { get; init; }
    }
}
