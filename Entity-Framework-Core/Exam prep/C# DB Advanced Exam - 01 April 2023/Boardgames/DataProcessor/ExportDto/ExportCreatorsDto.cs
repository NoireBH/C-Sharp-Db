using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto
{
	[XmlType("Creator")]
	public class ExportCreatorsDto
	{
		[XmlElement("CreatorName")]
		public string Name { get; set; } = null!;

		[XmlAttribute("BoardgamesCount")]
		public int BoardgamesCount { get; set; }

		[XmlArray("Boardgames")]
		public ExportCreatorsBoardgamesDto[] Boardgames { get; set; } = null!;
	}
}
