using AutoMapper;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Export.ExportSale;
using CarDealer.DTOs.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            //import

            CreateMap<ImportSuppliersDto, Supplier>();

            CreateMap<ImportPartsDto, Part>();

            CreateMap<ImportCustomerDto, Customer>();

            CreateMap<ImportSalesDto, Sale>();

            //export

            CreateMap<Car, ExportCarsDto>();

            CreateMap<Car, ExportBMWCars>();

            CreateMap<Supplier, ExportLocalSuppliersDto>()
                .ForMember(x => x.PartsCount, y => y.MapFrom(s => s.Parts.Count));


            CreateMap<Part, ExportCarPartDto>();

            CreateMap<Car, ExportCarsWIthPartsDto>()
                .ForMember(x => x.Parts, y => y.MapFrom(s => 
                    s.PartsCars
                    .Select(pc => pc.Part)
                    .OrderByDescending(p => p.Price)
                    .ToArray()));

            CreateMap<Sale, SaleDto>()
                .ForMember(x => x.Discount, y => y.MapFrom(s => (int)s.Discount))
                .ForMember(x => x.CustomerName, y => y.MapFrom(s => s.Customer.Name))
                .ForMember(x => x.PriceWithDiscount, y => y.MapFrom(s => Math.Round((double)(s.Car.PartsCars.Sum(p => p.Part.Price) * (1 - (s.Discount / 100))), 4)))
                .ForMember(x => x.Price, y => y.MapFrom(s => s.Car.PartsCars.Sum(p => p.Part.Price)));

            CreateMap<Car, CarDto>();
        }
    }
}
