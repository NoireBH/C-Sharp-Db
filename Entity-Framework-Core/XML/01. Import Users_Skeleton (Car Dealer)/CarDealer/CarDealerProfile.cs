using AutoMapper;
using CarDealer.DTOs.Export;
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
        }
    }
}
