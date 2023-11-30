using AutoMapper;
using Invoices.Data.Models;
using Invoices.DataProcessor.ExportDto;
using System.Globalization;

namespace Invoices
{
	public class InvoicesProfile : Profile
	{
		public InvoicesProfile()
		{
			CreateMap<Invoice, ExportClientInvoicesDto>()
				.ForMember(dest => dest.DueDate, opt =>
					opt.MapFrom(s => s.DueDate.ToString("d", CultureInfo.InvariantCulture)))
				.ForMember(dest => dest.Currency, opt =>
					opt.MapFrom(s => s.CurrencyType.ToString()));

			CreateMap<Client, ExportClientsDto>()
				.ForMember(dest => dest.Invoices, opt =>
					opt.MapFrom(s => s.Invoices
										.ToArray()
										.OrderBy(i => i.IssueDate)
										.ThenByDescending(i => i.DueDate)));

		}
	}
}
