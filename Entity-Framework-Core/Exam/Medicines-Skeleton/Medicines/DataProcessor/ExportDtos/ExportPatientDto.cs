using Medicines.Common;
using Medicines.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ExportDtos
{
	[XmlType("Patient")]
	public class ExportPatientDto
	{
		[Required]
		[XmlElement("Name")]
		public string FullName { get; set; } = null!;

		[Required]
		[XmlElement("AgeGroup")]
		public string AgeGroup { get; set; } = null!;

		[Required]
		[XmlAttribute("Gender")]
		public string Gender { get; set; } = null!;

		[XmlArray("Medicines")]
		public ExportPatientMedicineDto[] Medicines { get; set; } = null!;
	}
}
