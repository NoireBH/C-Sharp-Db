using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicines.Common
{
	public static class ValidationConstants
	{
		public class Pharmacy
		{
			public const int NameMinLength = 2;
			public const int NameMaxLength = 50;

			public const int PhoneNumberLength = 14;

			public const string PhoneRegex = @"^(\(\d{3}\) \d{3}-\d{4})$";
		}

		public class Medicine
		{
			public const int NameMinLength = 3;
			public const int NameMaxLength = 150;

			public const decimal PriceMin = 0.01m;
			public const decimal PriceMax = 1000.00m;

			public const int ProducerMinLength = 3;
			public const int ProducerMaxLength = 100;

			public const int CategoryMinEnum = 0;
			public const int CategoryMaxEnum = 4;
		}

		public class Patient
		{
			public const int FullNameMinLength = 5;
			public const int FullNameMaxLength = 100;

			public const int AgeMinEnum = 0;
			public const int AgeMaxEnum = 2;

			public const int GenderMinEnum = 0;
			public const int GenderMaxEnum = 1;
		}
	}
}
