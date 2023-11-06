using Microsoft.EntityFrameworkCore;
using Project.Entity.Models;
using Project.Entity.RequestFeatures;
using Project.Repositories.EfCore;
using Project.Repositories.Repository.Abstract;
using Project.Repositories.Repository.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Repositories.Repository.Concrete
{
    public sealed class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(DbContext databaseContext) : base(databaseContext)
        {
        }
        public async Task CreateSingleAsync(Product entity) =>await CreateAsync(entity);
        public async Task DeleteSingleAsync(Product entity) => await DeleteAsync(entity);
        public async Task UpdateSingleAsync(Product entity) => await UpdateAsync(entity);
        public async Task<CustomPagedList<Product>> GetAllAsync(ProductRequestParameters parameters, bool TrackChanges)
        {
            var data = await FindAllAsync(TrackChanges); 
            data = data
                .PriceRangeFilter(parameters.MinPrice, parameters.MaxPrice)
                .SearchByKeyword(parameters.SearchTerm)
                .SortByCustomFilter(parameters.OrderBy);
            return CustomPagedList<Product>.ToPagedList(data,parameters.PageNumber,parameters.PageSize);
        }
        public async Task<Product> GetByIdAsync(int Id, bool TrackChanges)
        {
            var data = await FindByConditionAsync(b => b.Id.Equals(Id), TrackChanges);
            return data.SingleOrDefault();
        }

    }
}
