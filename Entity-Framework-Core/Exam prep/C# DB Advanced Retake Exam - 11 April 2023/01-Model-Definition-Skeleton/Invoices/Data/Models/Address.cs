﻿using System.ComponentModel.DataAnnotations;
using static Invoices.Common.ValidationConstants.Address;
namespace Invoices.Data.Models
{
	public class Address
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(StreetNameMaxLength)]
		public string StreetName { get; set; } = null!;

		[Required]
		public int StreetNumber { get; set; }

		[Required]
		public string PostCode { get; set; } = null!;

		[Required]
		[MaxLength(CityMaxLength)]
		public string City { get; set; } = null!;

		[Required]
		[MaxLength(CountryMaxLength)]
		public string Country { get; set; } = null!;

		[Required]
		public int ClientId { get; set; }

		public Client Client { get; set; } = null!;
	}
}
