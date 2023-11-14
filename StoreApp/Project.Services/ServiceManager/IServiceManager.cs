using Project.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.ServiceManager
{
    public interface IServiceManager
    {
        public IProductService ProductService { get; }
        public IAuthenticationService AuthenticationService { get; }
    }
}
