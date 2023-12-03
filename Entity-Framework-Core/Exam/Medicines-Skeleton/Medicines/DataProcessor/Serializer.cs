namespace Medicines.DataProcessor
{
	using AutoMapper;
	using AutoMapper.QueryableExtensions;
	using Medicines.Data;
	using Medicines.Data.Models;
	using Medicines.Data.Models.Enums;
	using Medicines.DataProcessor.ExportDtos;
	using Medicines.Utulities;
	using Newtonsoft.Json;
	using System.Globalization;

	public class Serializer
	{
		private static XmlHelper XmlHelper;

		public static string ExportPatientsWithTheirMedicines(MedicinesContext context, string date)
		{
			XmlHelper = new XmlHelper();

			var config = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile<MedicinesProfile>();
			});

			DateTime dateParsed;

			var dateValue = DateTime.TryParse(date, out dateParsed);

			var patients = context.Patients
				.ToArray()
				.Where(p => p.PatientsMedicines
						.Any(m => m.Medicine.ProductionDate >= dateParsed))
				.Select(p => new ExportPatientDto
				{
					FullName = p.FullName,
					AgeGroup = p.AgeGroup.ToString(),
					Gender = p.Gender.ToString().ToLower(),
					Medicines = p.PatientsMedicines
						.Where(pm => pm.Medicine.ProductionDate >= dateParsed)
						.Select(pm => pm.Medicine)
						.OrderByDescending(m => m.ExpiryDate)
						.ThenBy(m => m.Price)
						.ToArray()
						.Select(m => new ExportPatientMedicineDto
						{
							Category = m.Category.ToString().ToLower(),
							Name = m.Name,
							Price = m.Price.ToString("F2"),
							Producer = m.Producer,
							BestBefore = m.ExpiryDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
						})
						.ToArray()
				})
				.OrderByDescending(p => p.Medicines.Length)
				.ThenBy(p => p.FullName)
				.ToArray();

			return XmlHelper.Serialize(patients, "Patients");
		}

		public static string ExportMedicinesFromDesiredCategoryInNonStopPharmacies(MedicinesContext context, int medicineCategory)
		{
			var medicines = context.Medicines
				.Where(m => m.Category == (Category)medicineCategory && m.Pharmacy.IsNonStop)
				.ToList()
				.Select(m => new
				{
					Name = m.Name,
					Price = m.Price.ToString("F2"),
					Pharmacy = new
					{
						Name = m.Pharmacy.Name,
						PhoneNumber = m.Pharmacy.PhoneNumber
					}

				})
				.OrderBy(m => m.Price)
				.ThenBy(m => m.Name);

			return JsonConvert.SerializeObject(medicines, Formatting.Indented);
		}
	}
}
