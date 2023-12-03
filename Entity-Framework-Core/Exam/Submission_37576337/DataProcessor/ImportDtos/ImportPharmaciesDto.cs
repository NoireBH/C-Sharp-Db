using Medicines.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ImportDtos
{
	[XmlType("Pharmacy")]
	public class ImportPharmaciesDto
	{
		[Required]
		[MinLength(ValidationConstants.Pharmacy.NameMinLength)]
		[MaxLength(ValidationConstants.Pharmacy.NameMaxLength)]
		[XmlElement("Name")]
		public string Name { get; set; } = null!;

		[Required]
		[StringLength(ValidationConstants.Pharmacy.PhoneNumberLength, MinimumLength = ValidationConstants.Pharmacy.PhoneNumberLength)]
		[XmlElement("PhoneNumber")]
		public string PhoneNumber { get; set; } = null!;

		[Required]
		[XmlAttribute("non-stop")]
		public string IsNonStop { get; set; }

		public ImportPharmacyMedicineDto[] Medicines { get; set; } = null!;
	}
}
