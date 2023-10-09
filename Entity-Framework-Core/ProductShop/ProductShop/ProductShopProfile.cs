using AutoMapper;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile() 
        {
            CreateMap<ImportUserDto, User>();

            CreateMap<ImportProductDto, Product>();

            CreateMap<ImportCategoryDto, Category>();

            CreateMap<ImportCategoryProductDto, CategoryProduct>();

            CreateMap<User, ExportSoldProductsDto>()
                .ForMember(x => x.SoldProducts, y => y.MapFrom(s => s.ProductsSold));
        }
    }
}
