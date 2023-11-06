using Project.Entity.Models;
using Project.Entity.RequestFeatures;
using Project.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.Repositories.Repository.Abstract
{
    public interface IProductRepository : IRepository<Product>
    {
        // additional functions are coming here (ex, paging, filtering)
        Task<CustomPagedList<Product>> GetAllAsync(ProductRequestParameters parameters, bool TrackChanges);
        Task<Product> GetByIdAsync(int Id, bool TrackChanges); 
        Task CreateSingleAsync(Product entity);
        Task UpdateSingleAsync(Product entity);
        Task DeleteSingleAsync(Product entity);
    }
}
