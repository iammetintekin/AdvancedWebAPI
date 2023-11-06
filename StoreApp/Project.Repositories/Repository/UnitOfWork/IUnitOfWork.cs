using Project.Repositories.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Repositories.Repository.UnitOfWork
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
        Task SaveAsync();
    }
}
