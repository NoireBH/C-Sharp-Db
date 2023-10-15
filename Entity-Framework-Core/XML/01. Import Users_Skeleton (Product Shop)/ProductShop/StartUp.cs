using AutoMapper;
using AutoMapper.Configuration.Annotations;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Export.ExportUsersAndProducts;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using ProductShop.Utilities;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            using ProductShopContext context = new ProductShopContext();

            string inputXml = @"../../../Datasets/categories-products.xml";

            string result = GetUsersWithProducts(context);
            Console.WriteLine(result);

        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            XmlHelper xmlHelper = new XmlHelper();
            ImportUsersDto[] usersDtos = xmlHelper.Deserialize<ImportUsersDto[]>(inputXml, "Users");



            ICollection<User> validUsers = new HashSet<User>();

            foreach (var userDto in usersDtos)
            {
                User user = mapper.Map<User>(userDto);
                validUsers.Add(user);
            }

            context.Users.AddRange(validUsers);
            context.SaveChanges();

            return $"Successfully imported {validUsers.Count}"; ;
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            XmlHelper xmlHelper = new XmlHelper();
            ImportProductsDto[] productsDtos = xmlHelper.Deserialize<ImportProductsDto[]>(inputXml, "Products");

            ICollection<Product> validProducts = new HashSet<Product>();

            foreach (var pDto in productsDtos)
            {
                if (string.IsNullOrWhiteSpace(pDto.Name) || pDto.BuyerId == null)
                {
                    continue;
                }

                Product product = mapper.Map<Product>(pDto);
                validProducts.Add(product);
            }

            context.Products.AddRange(validProducts);
            context.SaveChanges();

            return $"Successfully imported {validProducts.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            XmlHelper xmlHelper = new XmlHelper();
            ImportCategoriesDto[] categoriesDtos = xmlHelper.Deserialize<ImportCategoriesDto[]>(inputXml, "Categories");

            ICollection<Category> validCategories = new HashSet<Category>();

            foreach (var cDto in categoriesDtos)
            {
                if (string.IsNullOrWhiteSpace(cDto.Name))
                {
                    continue;
                }

                Category category = mapper.Map<Category>(cDto);
                validCategories.Add(category);
            }

            context.Categories.AddRange(validCategories);
            context.SaveChanges();

            return $"Successfully imported {validCategories.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            XmlHelper xmlHelper = new XmlHelper();
            ImportCategoryProductDto[] categoryProductsDtos =
                xmlHelper.Deserialize<ImportCategoryProductDto[]>(inputXml, "CategoryProducts");

            ICollection<CategoryProduct> validEntries = new HashSet<CategoryProduct>();

            var categories = context.Categories;
            var products = context.Products;


            foreach (var cpDto in categoryProductsDtos)
            {
                if (!categories.Any(c => c.Id == cpDto.CategoryId) || !products.Any(p => p.Id == cpDto.ProductId))
                {
                    continue;
                }

                CategoryProduct category = mapper.Map<CategoryProduct>(cpDto);
                validEntries.Add(category);
            }

            context.CategoryProducts.AddRange(validEntries);
            context.SaveChanges();

            return $"Successfully imported {validEntries.Count}";


        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();

            XmlHelper xmlHelper = new XmlHelper();

            var productsInRange = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Take(10)
                .ProjectTo<ExportProductsInRangeDto>(mapper.ConfigurationProvider)
                .ToArray();

            return xmlHelper.Serialize<ExportProductsInRangeDto[]>(productsInRange, "Products");
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();
            XmlHelper xmlHelper = new XmlHelper();

            var soldProducts = context.Users
                .Where(u => u.ProductsSold.Count > 0)
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Take(5)
                .ProjectTo<ExportUserWithSoldProductsDto>(mapper.ConfigurationProvider)
                .ToArray();

            return xmlHelper.Serialize<ExportUserWithSoldProductsDto[]>(soldProducts, "Users");
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();
            XmlHelper xmlHelper = new XmlHelper();

            var categoriesAndProductsInfo = context.Categories
                .ProjectTo<ExportCategoriesByProductsCountDto>(mapper.ConfigurationProvider)
                .OrderByDescending(c => c.Count)
                .ThenBy(c => c.TotalRevenue)
                .ToArray();

            return xmlHelper.Serialize<ExportCategoriesByProductsCountDto[]>(categoriesAndProductsInfo, "Categories");
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();
            XmlHelper xmlHelper = new XmlHelper();

            var usersInfo = context
                .Users
                .Where(u => u.ProductsSold.Any())
                .OrderByDescending(u => u.ProductsSold.Count)
                .Select(u => new UserInfo()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = new SoldProductsCount()
                    {
                        Count = u.ProductsSold.Count,
                        Products = u.ProductsSold.Select(p => new SoldProduct()
                        {
                            Name = p.Name,
                            Price = p.Price
                        })
                        .OrderByDescending(p => p.Price)
                        .ToArray()
                    }
                })
                .Take(10)
                .ToArray();

            ExportUserCountDto exportUserCountDto = new ExportUserCountDto()
            {
                Count = context.Users.Count(u => u.ProductsSold.Any()),
                Users = usersInfo
            };


            return xmlHelper.Serialize<ExportUserCountDto>(exportUserCountDto, "Users");

        }

        private static IMapper CreateMapper()
        {
            return new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            }));
        }
    }
}