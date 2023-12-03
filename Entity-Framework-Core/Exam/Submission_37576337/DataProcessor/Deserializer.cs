namespace Medicines.DataProcessor
{
	using Medicines.Data;
	using Medicines.Data.Models;
	using Medicines.Data.Models.Enums;
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
					if (context.PatientsMedicines.Any(m => m.MedicineId == medicineDto))
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					newPatient.PatientsMedicines.Add(new PatientMedicine { MedicineId = medicineDto });
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
			XmlHelper = new XmlHelper();
			StringBuilder sb = new StringBuilder();

			var PharmacyDtos = XmlHelper.Deserialize<ImportPharmaciesDto[]>(xmlString, "Pharmacies");

			var validPharmacies = new List<Pharmacy>();

			foreach (var pharmacyDto in PharmacyDtos)
			{
				if (!IsValid(pharmacyDto))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				bool parsedNonStop;

				if (!Boolean.TryParse(pharmacyDto.IsNonStop, out parsedNonStop))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				Pharmacy newPharmacy = new Pharmacy()
				{
					Name = pharmacyDto.Name,
					PhoneNumber = pharmacyDto.PhoneNumber,
					IsNonStop = parsedNonStop
				};

				var validMedicines = new List<Medicine>();

				foreach (var medicineDto in pharmacyDto.Medicines)
				{
					if (!IsValid(medicineDto))
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					if (medicineDto.ProductionDate >= medicineDto.ExpiryDate)
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					if (validMedicines.Any(m => m.Name == medicineDto.Name && m.Producer == medicineDto.Producer))
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					if (String.IsNullOrWhiteSpace(medicineDto.Producer))
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}



					Medicine newMedicine = new Medicine()
					{
						Name = medicineDto.Name,
						Price = medicineDto.Price,
						ProductionDate = medicineDto.ProductionDate,
						ExpiryDate = medicineDto.ExpiryDate,
						Producer = medicineDto.Producer,
						Category = (Category)medicineDto.Category
					};

					validMedicines.Add(newMedicine);
				}

				newPharmacy.Medicines = validMedicines;
				validPharmacies.Add(newPharmacy);

				sb.AppendLine(String.Format(SuccessfullyImportedPharmacy, newPharmacy.Name, newPharmacy.Medicines.Count));
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
