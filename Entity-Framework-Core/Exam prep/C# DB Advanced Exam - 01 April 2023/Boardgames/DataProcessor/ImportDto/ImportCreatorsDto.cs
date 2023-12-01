using Boardgames.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto
{
	[XmlType("Creator")]
	public class ImportCreatorsDto
	{
		[XmlElement("FirstName")]
		[Required]
		[StringLength(ValidationConstants.Creator.FirstNameMaxLength,
			MinimumLength = ValidationConstants.Creator.FirstNameMinLength)]
		public string FirstName { get; set; } = null!;

		[Required]
		[XmlElement("LastName")]
		[StringLength(ValidationConstants.Creator.LastNameMaxLength,
			MinimumLength = ValidationConstants.Creator.LastNameMinLength)]
		public string LastName { get; set; } = null!;

		[XmlArray("Boardgames")]
		public ImportCreatorsBoardgamesDto[] Boardgames { get; set; } = null!;
	}
}
