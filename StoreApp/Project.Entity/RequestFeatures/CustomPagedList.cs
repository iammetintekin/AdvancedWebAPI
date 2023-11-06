using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entity.RequestFeatures
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CustomPagedList<T> : List<T>
    {
        public MetaData MetaData { get; set; }
        public CustomPagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            MetaData = new MetaData()
            {
                TotalCount = count,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalPage = (count%pageSize) == 0 ? (count / pageSize): (count / pageSize) + 1
            };
            AddRange(items);
        }
        public static CustomPagedList<T> ToPagedList(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip(pageNumber-1)
                .Take(pageSize)
                .ToList();
            return new CustomPagedList<T>(items, count, pageNumber, pageSize);  
        }
    }
}
