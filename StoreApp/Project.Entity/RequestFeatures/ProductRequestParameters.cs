using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entity.RequestFeatures
{
    public class ProductRequestParameters:RequestParameters
    {
        public uint MinPrice { get; set; }
        public uint MaxPrice { get; set; } = 1000;
        public bool ValidPriceRange => MaxPrice > MinPrice?true:false;
        public  string? SearchTerm { get; set; }
        public ProductRequestParameters()
        {
            OrderBy = "id"; // default
        }
    }
}
