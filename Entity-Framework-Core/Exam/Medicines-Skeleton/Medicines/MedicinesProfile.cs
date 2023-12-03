namespace Medicines
{
	using AutoMapper;
	using Medicines.Data.Models;
	using Medicines.DataProcessor.ExportDtos;
	using System.Globalization;

	public class MedicinesProfile : Profile
	{
		// Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE OR RENAME THIS CLASS
		public MedicinesProfile()
		{

			CreateMap<PatientMedicine, ExportPatientDto>();

			CreateMap<Patient, ExportPatientDto>()
				.ForMember(dest => dest.FullName, opt =>
					opt.MapFrom(s => s.FullName))
				.ForMember(dest => dest.AgeGroup, opt =>
					opt.MapFrom(s => s.AgeGroup.ToString()))
				.ForMember(dest => dest.Gender, opt =>
					opt.MapFrom(s => s.Gender.ToString()))
				.ForMember(dest => dest.Medicines, opt =>
					opt.MapFrom(s => s.PatientsMedicines.Select(pm => pm.Medicine)
												.ToList()
												.OrderByDescending(m => m.ExpiryDate)
												.ThenBy(m => m.Price)));



			CreateMap<PatientMedicine, ExportPatientMedicineDto>()
				.ForMember(dest => dest.Category, opt =>
					opt.MapFrom(s => s.Medicine.Category.ToString()))
				.ForMember(dest => dest.BestBefore, opt =>
					opt.MapFrom(s => s.Medicine.ExpiryDate))
				.ForMember(dest => dest.Name, opt =>
					opt.MapFrom(s => s.Medicine.Name))
				.ForMember(dest => dest.Price, opt =>
					opt.MapFrom(s => s.Medicine.Price))
				.ForMember(dest => dest.Producer, opt =>
					opt.MapFrom(s => s.Medicine.Producer))
				.ForMember(dest => dest.BestBefore, opt =>
					opt.MapFrom(s => s.Medicine.ExpiryDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));

		}
	}
}
