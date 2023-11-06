using Project.Entity.DTOs.Product;
using Project.Entity.Models;
using Project.Entity.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.Abstract
{
    public interface IProductService
    {

        Task<(IEnumerable<ShapedEntity> products, MetaData meta)> GetAllAsync(ProductRequestParameters parameters, bool TrackChanges);
        Task<ProductDto> GetByIdAsync(int Id, bool TrackChanges);
        Task<ProductDto> CreateSingleAsync(CreateProductDto entity);
        Task UpdateSingleAsync(int Id, UpdateProductDto ProductDTO, bool TrackChanges);
        Task DeleteByIdAsync(int Id, bool TrackChanges);
        Task<Tuple<UpdateProductDto, Product>> GetOneProductForPatchAsync(int Id, bool TrackChanges);
        Task SaveChangesForPatch(UpdateProductDto updateProductDto, Product product);
        Task<List<ProductDto>> GetAllAsync(bool v);
    }
}
