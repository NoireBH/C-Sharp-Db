using Invoices.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using static Invoices.Common.ValidationConstants.Product;
namespace Invoices.Data.Models
{
	public class Product
	{
		public Product()
		{
			ProductsClients = new HashSet<ProductClient>();
		}

		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(NameMaxLength)]
		public string Name { get; set; } = null!;

		[Required]
		[Range((double)PriceMinLength, (double)PriceMaxLength)]
		public decimal Price { get; set; }

		[Required]
		public CategoryType CategoryType { get; set; }

		public virtual ICollection<ProductClient> ProductsClients { get; set; }
	}
}
