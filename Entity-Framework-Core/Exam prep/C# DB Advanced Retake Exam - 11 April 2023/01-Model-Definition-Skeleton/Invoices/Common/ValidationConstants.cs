using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Common
{
	public static class ValidationConstants
	{
		public class Product
		{
			public const int NameMinLength = 9;
			public const int NameMaxLength = 30;

			public const decimal PriceMinLength = 5m;
			public const decimal PriceMaxLength = 1000m;

		}

		public class Address
		{
			public const int StreetNameMinLength = 10;
			public const int StreetNameMaxLength = 20;

			public const int CityMinLength = 5;
			public const int CityMaxLength = 15;

			public const int CountryMinLength = 5;
			public const int CountryMaxLength = 15;
		}

		public class Invoice
		{
			public const int NumberMinLength = 1000000000;
			public const int NumberMaxLength = 1500000000;
		}

		public class Client
		{
			public const int NameMinLength = 10;
			public const int NameMaxLength = 25;

			public const int NumberVatMinLength = 10;
			public const int NumberVatMaxLength = 15;
		}
	}
}
