using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using CarDealer.Utilities;
using Microsoft.EntityFrameworkCore;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            var context = new CarDealerContext();
            string inputXml = @"../../../Datasets/sales.xml";

            string result = GetTotalSalesByCustomer(context);
            Console.WriteLine(result);

        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();
            XmlHelper xmlHelper = new XmlHelper();

            ImportSuppliersDto[] importedSuppliers = xmlHelper.Deserialize<ImportSuppliersDto[]>(inputXml, "Suppliers");

            List<Supplier> validSuppliers = new List<Supplier>();

            foreach (var supplierDto in importedSuppliers)
            {
                if (string.IsNullOrEmpty(supplierDto.Name))
                {
                    continue;
                }

                var supplier = mapper.Map<Supplier>(supplierDto);
                validSuppliers.Add(supplier);

            }

            context.Suppliers.AddRange(validSuppliers);
            context.SaveChanges();

            return $"Successfully imported {validSuppliers.Count}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();
            XmlHelper xmlHelper = new XmlHelper();

            ImportPartsDto[] importedPartsDtos = xmlHelper.Deserialize<ImportPartsDto[]>(inputXml, "Parts");
            var suppliers = context.Suppliers.ToArray();

            var validParts = new List<Part>();

            foreach (var supplierDto in importedPartsDtos)
            {
                if (!suppliers.Any(s => s.Id == supplierDto.SupplierId))
                {
                    continue;
                }

                var part = mapper.Map<Part>(supplierDto);
                validParts.Add(part);

            }

            context.Parts.AddRange(validParts);
            context.SaveChanges();

            return $"Successfully imported {validParts.Count}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();
            XmlHelper xmlHelper = new XmlHelper();

            ImportCarsDto[] CarsDtos = xmlHelper.Deserialize<ImportCarsDto[]>(inputXml, "Cars");
            var partsDbArray = context.Parts.ToArray();
            var validCars = new List<Car>();

            foreach (var carDto in CarsDtos)
            {
                Car car = new Car()
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TraveledDistance = carDto.TraveledDistance
                };



                foreach (var part in carDto.Parts.DistinctBy(p => p.PartId))
                {
                    if (!partsDbArray.Any(p => p.Id == part.PartId))
                    {
                        continue;
                    }

                    PartCar partCar = new PartCar()
                    {
                        PartId = part.PartId
                    };

                    car.PartsCars.Add(partCar);
                }

                validCars.Add(car);
            }

            context.Cars.AddRange(validCars);
            context.SaveChanges();

            return $"Successfully imported {validCars.Count}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();
            var xmlHelper = new XmlHelper();

            ImportCustomerDto[] importedCustomersDtos = xmlHelper.Deserialize<ImportCustomerDto[]>(inputXml, "Customers");

            ICollection<Customer> validCustomers = new HashSet<Customer>();

            foreach (var customerDto in importedCustomersDtos)
            {
                if (string.IsNullOrEmpty(customerDto.Name))

                {
                    continue;
                }

                Customer customer = mapper.Map<Customer>(customerDto);
                validCustomers.Add(customer);

            }

            context.Customers.AddRange(validCustomers);
            context.SaveChanges();

            return $"Successfully imported {validCustomers.Count}";

        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();
            var xmlHelper = new XmlHelper();

            ImportSalesDto[] importedSalesDtos = xmlHelper.Deserialize<ImportSalesDto[]>(inputXml, "Sales");

            ICollection<Sale> validSales = new HashSet<Sale>();

            foreach (var saleDto in importedSalesDtos)
            {
                if (!context.Cars.Any(c => c.Id == saleDto.CarId))
                {
                    continue;
                }

                Sale customer = mapper.Map<Sale>(saleDto);
                validSales.Add(customer);
            }

            context.Sales.AddRange(validSales);
            context.SaveChanges();

            return $"Successfully imported {validSales.Count}";
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            IMapper mapper = CreateMapper();
            var xmlHelper = new XmlHelper();

            var carsWithDistance = context.Cars
                .Where(c => c.TraveledDistance > 200000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ProjectTo<ExportCarsDto>(mapper.ConfigurationProvider)
                .ToArray();

            return xmlHelper.Serialize(carsWithDistance, "cars");
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            IMapper mapper = CreateMapper();
            var xmlHelper = new XmlHelper();

            var bmwCars = context.Cars
                .Where(c => c.Make == "BMW")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .ProjectTo<ExportBMWCars>(mapper.ConfigurationProvider)
                .ToArray();

            return xmlHelper.Serialize(bmwCars, "cars");
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            IMapper mapper = CreateMapper();
            var xmlHelper = new XmlHelper();

            ExportLocalSuppliersDto[] localSuppliers = context.Suppliers
                .Where(s => !s.IsImporter)
                .ProjectTo<ExportLocalSuppliersDto>(mapper.ConfigurationProvider)
                .ToArray();

            return xmlHelper.Serialize(localSuppliers, "suppliers");
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            IMapper mapper = CreateMapper();
            var xmlHelper = new XmlHelper();

            ExportCarsWIthPartsDto[] carsWithParts = context.Cars
                .OrderByDescending(c => c.TraveledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .ProjectTo<ExportCarsWIthPartsDto>(mapper.ConfigurationProvider)
                .ToArray();

            return xmlHelper.Serialize(carsWithParts, "cars");
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            IMapper mapper = CreateMapper();
            var xmlHelper = new XmlHelper();

            var tempDto = context.Customers
                .Where(c => c.Sales.Any())                              
                .Select(c => new
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count(),
                    SalesInfo = c.Sales.Select(s => new
                    {
                        Prices = c.IsYoungDriver 
                            ? s.Car.PartsCars.Sum(p => Math.Round((double)p.Part.Price * 0.95, 2)) 
                            : s.Car.PartsCars.Sum(p => (double)p.Part.Price)                      
                    }).ToArray(),                                        
                })
                .ToArray();

            TotalSalesByCustomerDto[] totalSalesDtos = tempDto
                .OrderByDescending(t => t.SalesInfo.Sum(s => s.Prices))
                .Select(t => new TotalSalesByCustomerDto()
                {
                    FullName = t.FullName,
                    BoughtCars = t.BoughtCars,
                    SpentMoney = t.SalesInfo.Sum(s => s.Prices).ToString("f2")                
                })                
                .ToArray();

            return xmlHelper.Serialize(totalSalesDtos, "customers");
        }

        private static IMapper CreateMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });

            var mapper = new Mapper(config);

            return mapper;
        }
    }
}