using Microsoft.AspNetCore.Http;
using Project.Entity.DTOs.Product;
using Project.Entity.LinkModels;
using System;
namespace Project.Services.Abstract
{
    public interface IProductLinksService
    {
        LinkResponse TryGenerateLinks(IEnumerable<ProductDto> products, string fields, HttpContext context);
    }
}
