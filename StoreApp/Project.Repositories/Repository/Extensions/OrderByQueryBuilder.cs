using Project.Entity.Models;
using System;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Project.Repositories.Repository.Extensions
{
    public static class OrderByQueryBuilder
    {
        public static string CreateOrderQuery<T>(string orderByQueryString)
        {
            var PreparedQueries = orderByQueryString.Trim().Split(','); // title, date desc, id

            var entityProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var QueryBuilder = new StringBuilder();

            foreach (var param in PreparedQueries)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var propInfoFromQuery = param.Split(' ')[0]; // date desc

                var objectProperty = entityProperties.FirstOrDefault(pi => pi.Name.Equals(propInfoFromQuery, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty is null)
                    continue;

                //boşluk_desc içeriyorsa tersine, değilse normal yön
                var direction = param.EndsWith(" desc") ? "descending" : "ascending";

                QueryBuilder.Append($"{objectProperty.Name.ToString()} {direction},");

            }
            var lastOrderQuery = QueryBuilder.ToString().TrimEnd(',', ' ');

            return lastOrderQuery; // linq dynamic core 
        }
    }
}
