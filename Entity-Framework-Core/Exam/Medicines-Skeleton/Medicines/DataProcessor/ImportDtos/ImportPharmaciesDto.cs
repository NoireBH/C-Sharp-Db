using Medicines.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
		[Column(TypeName = "char")]
		[StringLength(ValidationConstants.Pharmacy.PhoneNumberLength)]
		[XmlElement("PhoneNumber")]
		public string PhoneNumber { get; set; } = null!;

		[Required]
		[XmlAttribute("non-stop")]
		public string IsNonStop { get; set; }

		[XmlArray("Medicines")]
		public ImportPharmacyMedicineDto[] Medicines { get; set; }
	}
}
