using Project.Entity.Models; 
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace Project.Repositories.Repository.Extensions
{
    public static class ProductRepositoryExtensions
    {
        // this yazılan kısım parametrede görünmüyor. çağırıldığında uygulanan fonksiyonlar böyle tanımlanır.
        public static IQueryable<Product> PriceRangeFilter(this IQueryable<Product> query, uint minPrice, uint maxPrice)
        {
            return query.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
        }
        public static IQueryable<Product> SearchByKeyword(this IQueryable<Product> query, string keyword)
        {
            if(string.IsNullOrWhiteSpace(keyword))
                return query;

            var lowerCaseTerm = keyword.Trim().ToLower();
            return query.Where(p => p.Name.ToLower().Contains(lowerCaseTerm));
        }
        public static IQueryable<Product> SortByCustomFilter(this IQueryable<Product> query, string orderByQueryString)
        {
            if(string.IsNullOrWhiteSpace(orderByQueryString))
                return query.OrderBy(s=>s.Id);

            var QueryBuilderForProduct = OrderByQueryBuilder.CreateOrderQuery<Product>(orderByQueryString);
             
            if(QueryBuilderForProduct is  null)
                return query.OrderBy(a=>a.Id); 
            return query.OrderBy(QueryBuilderForProduct); // linq dynamic core 
        }
    }
}
