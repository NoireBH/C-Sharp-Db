using Boardgames.Common;
using Boardgames.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto
{
	[XmlType("Boardgame")]
	public class ImportCreatorsBoardgamesDto
	{
		[Required]
		[StringLength(ValidationConstants.Boardgame.NameMaxLength,
			MinimumLength = ValidationConstants.Boardgame.NameMinLength)]
		[XmlElement("Name")]
		public string Name { get; set; } = null!;

		[Required]
		[Range(ValidationConstants.Boardgame.RatingMinLength, ValidationConstants.Boardgame.RatingMaxLength)]
		[XmlElement("Rating")]
		public double Rating { get; set; }

		[Required]
		[Range(ValidationConstants.Boardgame.YearPublishedMin
			, ValidationConstants.Boardgame.YearPublishedMax)]
		[XmlElement("YearPublished")]
		public int YearPublished { get; set; }

		[Required]
		[Range(0, 4)]
		[XmlElement("CategoryType")]
		public int CategoryType { get; set; }

		[Required]
		[XmlElement("Mechanics")]
		public string Mechanics { get; set; } = null!;
	}
}
