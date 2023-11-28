using Invoices.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using static Invoices.Common.ValidationConstants.Invoice;
namespace Invoices.DataProcessor.ImportDto
{
	public class ImportInvoicesDto
	{
		[Required]
		[Range(NumberMinLength, NumberMaxLength)]
		public int Number { get; set; }

		[Required]
		public DateTime IssueDate { get; set; }

		[Required]
		public DateTime DueDate { get; set; }

		[Required]
		public decimal Amount { get; set; }

		[Required]
		[Range(0, 2)]
		public CurrencyType CurrencyType { get; set; }

		[Required]
		public int ClientId { get; set; }
	}
}
