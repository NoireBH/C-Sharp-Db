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
            CreateMap<ImportCarsDto, Car>();
            CreateMap<ImportCustomersDto, Customer>();
            CreateMap<ImportSalesDto, Sale>();

            //export

            CreateMap<Customer, ImportCustomersDto>();

            CreateMap<Car, ExportCarsDto>()
                .ForMember(x => x.TraveledDistance, y => y.MapFrom(s => s.TravelledDistance));

            CreateMap<Car, ExportCarWithParts>()
                .ForMember(x => x.TraveledDistance, y => y.MapFrom(s => s.TravelledDistance));
        }
    }
}
