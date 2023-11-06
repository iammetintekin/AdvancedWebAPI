using Project.Repositories.EfCore;
using Project.Repositories.Repository.Abstract;
using Project.Repositories.Repository.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Repositories.Repository.UnitOfWork
{
    // normalde bir class içerisinde başka bir class newlenmez.
    // sıkı bağlı bir uygulama geliştirmiş oluruz.
    // lazy loadinge çevireceğiz
    // public IProductRepository Products => new ProductRepository(_dbContext);

    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _dbContext;
        private readonly Lazy<IProductRepository> _productRepository;
        public UnitOfWork(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
            _productRepository = new Lazy<IProductRepository>(()=>new ProductRepository(_dbContext));
        } 
        public IProductRepository Products => _productRepository.Value;

        public async Task SaveAsync()
        {
           await _dbContext.SaveChangesAsync();
        }
    }
}
