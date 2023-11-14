using AutoMapper;
using Project.Entity.DTOs.Identity;
using Project.Entity.DTOs.Product;
using Project.Entity.Models;

namespace WEBAPI.Utilities.AutoMapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<UpdateProductDto, Product>();
            CreateMap<Product, UpdateProductDto>();
            CreateMap<Product, ProductDto>();
            CreateMap<CreateProductDto, Product>();
            CreateMap<UserForRegistrationDto, User>();
        }
    }
}
