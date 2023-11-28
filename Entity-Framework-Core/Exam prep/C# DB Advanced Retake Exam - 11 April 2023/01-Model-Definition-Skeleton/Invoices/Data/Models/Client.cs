using System.ComponentModel.DataAnnotations;
using static Invoices.Common.ValidationConstants.Client;
namespace Invoices.Data.Models
{
	public class Client
	{
		public Client()
		{
			Invoices = new HashSet<Invoice>();
			Addresses = new HashSet<Address>();
			ProductsClients = new HashSet<ProductClient>();
		}

		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(NameMaxLength)]
		public string Name { get; set; } = null!;

		[Required]
		[MaxLength(NumberVatMaxLength)]
		public string NumberVat { get; set; } = null!;

		public ICollection<Invoice> Invoices { get; set; }

		public ICollection<Address> Addresses { get; set; }

		public ICollection<ProductClient> ProductsClients { get; set; }
	}
}
