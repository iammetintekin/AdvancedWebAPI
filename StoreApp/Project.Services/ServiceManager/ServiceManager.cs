using AutoMapper;
using Project.Entity.DTOs.Product;
using Project.Repositories.Repository.UnitOfWork;
using Project.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.ServiceManager
{
    /// <summary>
    /// Uses lazy loading.
    /// </summary>
    public class ServiceManager : IServiceManager
    {
        public ServiceManager(IUnitOfWork db, ILoggerService logger, IMapper mapper, IDataShaper<ProductDto> shaperForProduct)
        {
            _productService = new Lazy<IProductService> (()=> new ProductService(db,logger, mapper, shaperForProduct));
        }
        private readonly Lazy<IProductService> _productService;
        public IProductService ProductService => _productService.Value;

    }
}
