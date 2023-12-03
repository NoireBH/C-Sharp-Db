using Medicines.Common;
using Medicines.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ExportDtos
{
	[XmlType("Medicine")]
	public class ExportPatientMedicineDto
	{
		[Required]
		[XmlElement("Name")]
		public string Name { get; set; } = null!;

		[Required]
		[XmlElement("Price")]
		public string Price { get; set; } = null!;

		[Required]
		[XmlAttribute("Category")]
		public string Category { get; set; } = null!;

		[Required]
		[XmlElement("Producer")]
		public string Producer { get; set; } = null!;

		[Required]
		[XmlElement("BestBefore")]
		public string BestBefore { get; set; } = null!;

	}
}
