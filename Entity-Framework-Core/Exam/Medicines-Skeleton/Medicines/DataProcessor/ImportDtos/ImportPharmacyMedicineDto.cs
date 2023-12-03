using Medicines.Common;
using Medicines.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ImportDtos
{
	[XmlType("Medicine")]
	public class ImportPharmacyMedicineDto
	{
		[Required]
		[MinLength(ValidationConstants.Medicine.NameMinLength)]
		[MaxLength(ValidationConstants.Medicine.NameMaxLength)]
		public string Name { get; set; } = null!;

		[Required]
		[Range((double)ValidationConstants.Medicine.PriceMin, (double)ValidationConstants.Medicine.PriceMax)]
		public decimal Price { get; set; }

		[Required]
		[Range(ValidationConstants.Medicine.CategoryMinEnum, ValidationConstants.Medicine.CategoryMaxEnum)]
		[XmlAttribute("category")]
		public int Category { get; set; }

		[Required]
		public string ProductionDate { get; set; } = null!;

		[Required]
		public string ExpiryDate { get; set; } = null!;
		[Required]
		[MinLength(ValidationConstants.Medicine.ProducerMinLength)]
		[MaxLength(ValidationConstants.Medicine.ProducerMaxLength)]
		public string Producer { get; set; } = null!;
	}
}
