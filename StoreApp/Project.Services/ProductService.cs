using AutoMapper;
using Project.Entity.DTOs.Product;
using Project.Entity.Exceptions;
using Project.Entity.Models;
using Project.Entity.RequestFeatures;
using Project.Repositories.Repository.UnitOfWork;
using Project.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Project.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _db;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        private readonly IDataShaper<ProductDto> _dataShaper;

        public ProductService(IUnitOfWork db, ILoggerService logger, IMapper mapper, IDataShaper<ProductDto> dataShaper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _dataShaper = dataShaper;
        }

        public async Task<ProductDto> CreateSingleAsync(CreateProductDto entity)
        { 
            if (entity is null)
                throw new EntityIsNullException<Product>(); 

            var mapped = _mapper.Map<Product>(entity);

            await _db.Products.CreateSingleAsync(mapped);
            await _db.SaveAsync(); 

            var return_mapped = _mapper.Map<ProductDto>(mapped);
            return return_mapped;
        }
         
        public async Task DeleteByIdAsync(int Id, bool TrackChanges)
        {
            var entity = await CheckDataIfExistById(Id, TrackChanges);
            await _db.Products.DeleteSingleAsync(entity);
            await _db.SaveAsync();
        }

        public async Task<(IEnumerable<ShapedEntity> products, MetaData meta)> GetAllAsync(ProductRequestParameters parameters, bool TrackChanges)
        {
            if (!parameters.ValidPriceRange)
                throw new OutOfRangeException(nameof(ProductDto.Price));
            var dataIncludedMeta = await _db.Products.GetAllAsync(parameters, TrackChanges);
            var list = _mapper.Map<IEnumerable<ProductDto>>(dataIncludedMeta);
            var shapedData = _dataShaper.ShapeData(list, parameters.Fields);
            // headers da kullanılacak
            return (shapedData, dataIncludedMeta.MetaData);
        }

        public async Task<List<ProductDto>> GetAllAsync(bool v)
        {
            var dataIncludedMeta = await _db.Products.FindAllAsync(v);
            throw new NotImplementedException();
        }

        public async Task<ProductDto> GetByIdAsync(int Id, bool TrackChanges)
        {
            var entity = await CheckDataIfExistById(Id, TrackChanges); 
            var mapped = _mapper.Map<ProductDto>(entity);

            return mapped;
        }

        public async Task<Tuple<UpdateProductDto, Product>> GetOneProductForPatchAsync(int Id, bool TrackChanges)
        {
            var entity = await CheckDataIfExistById(Id, TrackChanges); 
            var return_mapped = _mapper.Map<UpdateProductDto>(entity);
            return new Tuple<UpdateProductDto, Product>(return_mapped, entity);
        }

        public async Task SaveChangesForPatch(UpdateProductDto updateProductDto, Product product)
        {
            _mapper.Map(updateProductDto,product);
            await _db.SaveAsync(); 
        }

        public async Task UpdateSingleAsync(int Id, UpdateProductDto ProductDTO, bool TrackChanges)
        {
            var product = await CheckDataIfExistById(Id, TrackChanges);
            product = _mapper.Map<Product>(ProductDTO);
            //we don't need to assign props from entity if tracking is true.
            await _db.Products.UpdateSingleAsync(product);
            await _db.SaveAsync();
        }
        private async Task<Product> CheckDataIfExistById(int Id, bool TrackChanges)
        {
            var entity = await _db.Products.GetByIdAsync(Id, TrackChanges);
            if (entity is null)
            {
                throw new EntityNotFoundException<Product>(Id);
            }
            return entity;
        }
    }
}
