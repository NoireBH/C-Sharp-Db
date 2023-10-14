using AutoMapper;
using AutoMapper.Configuration.Annotations;
using ProductShop.Data;
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

            string inputXml = @"../../../Datasets/users.xml";
            string result = ImportUsers(context, inputXml);

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
                if (string.IsNullOrWhiteSpace(pDto.Name))
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

        private static IMapper CreateMapper()
        {
            return new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            }));
        }
    }
}