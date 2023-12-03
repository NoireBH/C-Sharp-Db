using Medicines.Common;
using Medicines.Data.Models.Enums;
using Medicines.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicines.DataProcessor.ImportDtos
{
	public class ImportPatientsDto
	{
		[Required]
		[MinLength(ValidationConstants.Patient.FullNameMinLength)]
		[MaxLength(ValidationConstants.Patient.FullNameMaxLength)]
		public string FullName { get; set; } = null!;

		[Required]
		[Range(ValidationConstants.Patient.AgeMinEnum, ValidationConstants.Patient.AgeMaxEnum)]
		public int AgeGroup { get; set; }

		[Required]
		[Range(ValidationConstants.Patient.GenderMinEnum, ValidationConstants.Patient.GenderMaxEnum)]
		public int Gender { get; set; }

		public int[] Medicines { get; set; }
	}
}
