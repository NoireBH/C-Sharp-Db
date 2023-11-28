using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Invoices.Common.ValidationConstants.Client;

namespace Invoices.DataProcessor.ImportDto
{
	[XmlType("Client")]
	public class ImportClientsDto
	{
		[Required]
		[XmlElement("Name")]
		[StringLength(NameMaxLength, MinimumLength = NameMinLength)]
		public string Name { get; set; } = null!;

		[Required]
		[XmlElement("NumberVat")]
		[StringLength(NumberVatMaxLength, MinimumLength = NumberVatMinLength)]
		public string NumberVat { get; set; } = null!;

		[XmlArray("Addresses")]
		public ImportAddressesDto[] Addresses { get; set; } = null!;
	}

}
