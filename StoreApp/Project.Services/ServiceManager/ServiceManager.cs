using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Project.Entity.DTOs.Product;
using Project.Entity.Models;
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
        public ServiceManager(IUnitOfWork db, ILoggerService logger, IConfiguration configuration, IMapper mapper, IDataShaper<ProductDto> shaperForProduct, UserManager<User> userManager)
        {
            _productService = new Lazy<IProductService> (()=> new ProductService(db,logger, mapper, shaperForProduct));
            _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(logger, mapper, userManager, configuration));
        }
        private readonly Lazy<IProductService> _productService; 
        private readonly Lazy<IAuthenticationService> _authenticationService;

        public IProductService ProductService => _productService.Value;
        public IAuthenticationService AuthenticationService => _authenticationService.Value;
    }
}
