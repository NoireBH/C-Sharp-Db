using AutoMapper;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Export.ExportUsersAndProducts;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            //import
            CreateMap<ImportUsersDto, User>();
            CreateMap<ImportProductsDto, Product>();
            CreateMap<ImportCategoriesDto, Category>();
            CreateMap<ImportCategoryProductDto, CategoryProduct>();

            //export
            CreateMap<Product, ExportProductsInRangeDto>()
                .ForMember(x => x.Buyer, y => y.MapFrom(s => s.Buyer.FirstName + " " + s.Buyer.LastName));

            CreateMap<User, ExportUserWithSoldProductsDto>()
                .ForMember(x => x.SoldProducts, y => y.MapFrom(s => s.ProductsSold));

            CreateMap<Product, ExportSoldProductDto>();

            CreateMap<Category, ExportCategoriesByProductsCountDto>()
                .ForMember(x => x.Count, y => y.MapFrom(s => s.CategoryProducts.Count))
                .ForMember(x => x.AveragePrice, y => y.MapFrom(s => s.CategoryProducts.Average(x => x.Product.Price)))
                .ForMember(x => x.TotalRevenue, y => y.MapFrom(s => s.CategoryProducts.Sum(x => x.Product.Price)));

            CreateMap<User, ExportUsersWithTheirSoldProductsDto>()
                .ForMember(x => x.SoldProducts, y => y.MapFrom(s => s.ProductsSold));

            
        }
    }
}
