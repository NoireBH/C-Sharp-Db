namespace Medicines.DataProcessor
{
	using Medicines.Data;
	using Medicines.Data.Models;
	using Medicines.Data.Models.Enums;
	using Medicines.DataProcessor.ExportDtos;
	using Medicines.DataProcessor.ImportDtos;
	using Medicines.Utulities;
	using Newtonsoft.Json;
	using System.ComponentModel.DataAnnotations;
	using System.Globalization;
	using System.Text;

	public class Deserializer
	{
		private static XmlHelper? XmlHelper;

		private const string ErrorMessage = "Invalid Data!";
		private const string SuccessfullyImportedPharmacy = "Successfully imported pharmacy - {0} with {1} medicines.";
		private const string SuccessfullyImportedPatient = "Successfully imported patient - {0} with {1} medicines.";

		public static string ImportPatients(MedicinesContext context, string jsonString)
		{
			StringBuilder sb = new StringBuilder();

			var patientDtos = JsonConvert.DeserializeObject<ImportPatientsDto[]>(jsonString);

			var validPatients = new List<Patient>();

			foreach (var patientDto in patientDtos)
			{
				if (!IsValid(patientDto))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var newPatient = new Patient()
				{
					FullName = patientDto.FullName,
					AgeGroup = (AgeGroup)patientDto.AgeGroup,
					Gender = (Gender)patientDto.Gender
				};

				foreach (var medicineDto in patientDto.Medicines)
				{

					if (newPatient.PatientsMedicines.Any(x => x.MedicineId == medicineDto))
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					newPatient.PatientsMedicines.Add(new PatientMedicine()
					{
						Patient = newPatient,
						MedicineId = medicineDto
					});
				}

				validPatients.Add(newPatient);
				sb.AppendLine(String.Format(SuccessfullyImportedPatient, newPatient.FullName, newPatient.PatientsMedicines.Count));

			}

			context.Patients.AddRange(validPatients);
			context.SaveChanges();
			return sb.ToString().TrimEnd();
		}

		public static string ImportPharmacies(MedicinesContext context, string xmlString)
		{
			StringBuilder sb = new StringBuilder();
			XmlHelper = new XmlHelper();
			var pharmacyDtos = XmlHelper.Deserialize<ImportPharmaciesDto[]>(xmlString, "Pharmacies");

			ICollection<Pharmacy> validPharmacies = new List<Pharmacy>();

			foreach (var phDto in pharmacyDtos)
			{
				if (!IsValid(phDto))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				bool parsedNonStop;

				if (!Boolean.TryParse(phDto.IsNonStop, out parsedNonStop))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				Pharmacy pharmacy = new Pharmacy()
				{
					Name = phDto.Name,
					PhoneNumber = phDto.PhoneNumber,
					IsNonStop = parsedNonStop,
				};

				foreach (var medDto in phDto.Medicines)
				{
					if (!IsValid(medDto))
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					DateTime medicineProductionDate;
					bool isProductionDateValid = DateTime
						.TryParseExact(medDto.ProductionDate, "yyyy-MM-dd", CultureInfo
						.InvariantCulture, DateTimeStyles.None, out medicineProductionDate);

					if (!isProductionDateValid)
					{
						sb.Append(ErrorMessage);
						continue;
					}

					DateTime medicineExpityDate;
					bool isExpityDateValid = DateTime
						.TryParseExact(medDto.ExpiryDate, "yyyy-MM-dd", CultureInfo
						.InvariantCulture, DateTimeStyles.None, out medicineExpityDate);

					if (!isExpityDateValid)
					{
						sb.Append(ErrorMessage);
						continue;
					}

					if (medicineProductionDate >= medicineExpityDate)
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					if (pharmacy.Medicines.Any(x => x.Name == medDto.Name && x.Producer == medDto.Producer))
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					Medicine medicine = new Medicine()
					{
						Name = medDto.Name,
						Price = medDto.Price,
						Category = (Category)medDto.Category,
						ProductionDate = medicineProductionDate,
						ExpiryDate = medicineExpityDate,
						Producer = medDto.Producer,
					};

					pharmacy.Medicines.Add(medicine);
				}

				validPharmacies.Add(pharmacy);
				sb.AppendLine(string
					.Format(SuccessfullyImportedPharmacy, pharmacy.Name, pharmacy.Medicines.Count));
			}
			context.Pharmacies.AddRange(validPharmacies);
			context.SaveChanges();
			return sb.ToString().TrimEnd();
		}

		private static bool IsValid(object dto)
		{
			var validationContext = new ValidationContext(dto);
			var validationResult = new List<ValidationResult>();

			return Validator.TryValidateObject(dto, validationContext, validationResult, true);
		}
	}
}