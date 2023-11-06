using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entity.RequestFeatures
{
    public abstract class RequestParameters
    {
        const int maxPageSize = 50;

        //full prop
        private int _pageSize;
        public int PageSize
        {
            get { return _pageSize; }
            // 50 yi geçmesin page_size
            set { _pageSize = value > maxPageSize ? maxPageSize:value; }
        }
        public int PageNumber { get; set; }
        public string? OrderBy { get; set; }
        public string? Fields { get; set; } // used for data shapping on props which user wants. 
    }
}
