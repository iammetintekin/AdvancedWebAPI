using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entity.LinkModels
{
    public class LinkResponse
    {
        public bool HasLinks { get; set; }
        public List<Models.Entity> ShapedEntites { get; set; }
        public LinkCollectionWrapper<Models.Entity> LinkedEntites { get; set; }
        public LinkResponse()
        {
            ShapedEntites = new();
            LinkedEntites = new();
        }
    }
}
