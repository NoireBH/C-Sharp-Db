using Boardgames.Common;
using Boardgames.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boardgames.DataProcessor.ImportDto
{
	public class ImportSellersDto
	{
		[Required]
		[StringLength(ValidationConstants.Seller.NameMaxLength,
			MinimumLength = ValidationConstants.Seller.NameMinLength)]
		public string Name { get; set; } = null!;

		[Required]
		[StringLength(ValidationConstants.Seller.AddressMaxLength,
			MinimumLength = ValidationConstants.Seller.AddressMinLength)]
		public string Address { get; set; } = null!;

		[Required]
		public string Country { get; set; } = null!;

		[Required]
		[RegularExpression(ValidationConstants.Seller.WebsiteRegex)]
		public string Website { get; set; } = null!;

		public int[] Boardgames { get; set; } = null!;
	}
}
