using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boardgames.Common
{
	public static class ValidationConstants
	{
		public class Boardgame
		{
			public const int NameMinLength = 10;
			public const int NameMaxLength = 20;

			public const double RatingMinLength = 1;
			public const double RatingMaxLength = 10;

			public const int YearPublishedMin = 2018;
			public const int YearPublishedMax = 2023;
		}

		public class Seller
		{
			public const int NameMinLength = 5;
			public const int NameMaxLength = 20;

			public const int AddressMinLength = 2;
			public const int AddressMaxLength = 30;

			public const string WebsiteRegex = @"^www\.[a-zA-Z0-9\-]+\.com$";
		}

		public class Creator
		{
			public const int FirstNameMinLength = 2;
			public const int FirstNameMaxLength = 7;

			public const int LastNameMinLength = 2;
			public const int LastNameMaxLength = 7;
		}
	}
}
