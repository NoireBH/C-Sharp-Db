using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Invoices.Common.ValidationConstants.Address;

namespace Invoices.DataProcessor.ImportDto
{
	[XmlType("Address")]
	public class ImportAddressesDto
	{
		[Required]
		[StringLength(StreetNameMaxLength, MinimumLength = StreetNameMinLength)]
		public string StreetName { get; set; } = null!;

		[Required]
		public int StreetNumber { get; set; }

		[Required]
		public string PostCode { get; set; } = null!;

		[Required]
		[StringLength(CityMaxLength, MinimumLength = CityMinLength)]
		public string City { get; set; } = null!;

		[Required]
		[StringLength(CountryMaxLength, MinimumLength = CountryMinLength)]
		public string Country { get; set; } = null!;
	}
}
