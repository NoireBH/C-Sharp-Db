using Invoices.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using static Invoices.Common.ValidationConstants.Product;
namespace Invoices.DataProcessor.ImportDto
{
	public class ImportProductsDto
	{
		[Required]
		[StringLength(NameMaxLength, MinimumLength = NameMinLength)]
		public string Name { get; set; } = null!;

		[Required]
		[Range((double)PriceMinLength, (double)PriceMaxLength)]
		public decimal Price { get; set; }

		[Required]
		[Range(0, 4)]
		public CategoryType CategoryType { get; set; }

		public int[] Clients { get; set; } = null!;
	}
}
