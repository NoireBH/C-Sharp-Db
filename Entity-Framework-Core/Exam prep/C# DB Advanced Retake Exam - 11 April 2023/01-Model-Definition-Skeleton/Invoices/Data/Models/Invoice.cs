﻿using Invoices.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Invoices.Data.Models
{
	public class Invoice
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int Number {  get; set; }

		[Required]
		public DateTime IssueDate { get; set; }

		[Required]
		public DateTime DueDate { get; set; }

		[Required]
		public decimal Amount { get; set; }

		[Required]
		public CurrencyType CurrencyType { get; set; }

		[Required]
		public int ClientId { get; set; }

		public Client Client { get; set; } = null!;

	}
}
