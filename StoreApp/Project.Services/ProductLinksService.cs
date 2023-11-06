using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;
using Project.Entity.DTOs.Product;
using Project.Entity.LinkModels;
using Project.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services
{
    public class ProductLinksService : IProductLinksService
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IDataShaper<ProductDto> _dataShaper;

        public ProductLinksService(LinkGenerator linkGenerator, IDataShaper<ProductDto> dataShaper)
        {
            _linkGenerator = linkGenerator;
            _dataShaper = dataShaper;
        }

        //spahed ve linked data üreteceğiz
        public LinkResponse TryGenerateLinks(IEnumerable<ProductDto> products, string fields, HttpContext context)
        {
            var shapedProducts = ShapeData(products,fields);

            if (ShouldGenerateLinks(context))
               return ReturnLinkedProducts(products, fields, context, shapedProducts);

            return ReturnShapedProducts(shapedProducts);
        }

        private LinkResponse ReturnLinkedProducts(IEnumerable<ProductDto> products, string fields, HttpContext context, List<Entity.Models.Entity> shapedProducts)
        {
            var productDtos = products.ToList();
            for (int i = 0; i<productDtos.Count(); i++)
            {
                var productLinks = CreateForBook(context, productDtos[i], fields);
                shapedProducts[i].Add("Links", productLinks);
            }
            var productCollection = new LinkCollectionWrapper<Entity.Models.Entity>(shapedProducts);
            return new LinkResponse { HasLinks = true, LinkedEntites = productCollection};
        }

        private List<Link> CreateForBook(HttpContext context, ProductDto productDto, string fields)
        {
            var links = new List<Link>()
            {
                new Link("a1","a2","a3"),
                new Link("b1","b2","b3")
            };

            return links;
        }

        private LinkResponse ReturnShapedProducts(List<Entity.Models.Entity> shapedProducts)
        {
            return new LinkResponse { HasLinks = false, ShapedEntites = shapedProducts };
        }

        private bool ShouldGenerateLinks(HttpContext httpContext)
        {
            var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];
            return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas",StringComparison.InvariantCultureIgnoreCase); 
        }

        private List<Project.Entity.Models.Entity> ShapeData(IEnumerable<ProductDto> products, string fields) 
        { 
            return _dataShaper.ShapeData(products, fields).Select(s=>s.Entity).ToList(); 
        }
    }
}
