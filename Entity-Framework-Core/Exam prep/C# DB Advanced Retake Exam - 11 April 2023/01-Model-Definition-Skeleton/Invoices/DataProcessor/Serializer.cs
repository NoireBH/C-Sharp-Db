namespace Invoices.DataProcessor
{
	using AutoMapper;
	using AutoMapper.QueryableExtensions;
	using Invoices.Data;
	using Invoices.DataProcessor.ExportDto;
	using Newtonsoft.Json;
	using SoftJail.Utilities;
	using System.Text;
	using System.Xml.Linq;
	using System.Xml.Serialization;

	public class Serializer
	{
		private readonly XmlHelper xmlHelper;

		public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
		{
			StringBuilder sb = new StringBuilder();
			XmlHelper xmlHelper = new XmlHelper();

			var config = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile<InvoicesProfile>();
			});

			ExportClientsDto[] clientsDtos = context
				.Clients
				.Where(c => c.Invoices.Any(ci => ci.IssueDate >= date))
				.ProjectTo<ExportClientsDto>(config)
				.OrderByDescending(c => c.InvoicesCount)
				.ThenBy(c => c.Name)
				.ToArray();

			return xmlHelper.Serialize(clientsDtos, "Clients");
			
		}

		public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
		{

			var top5Products = context.Products
				.ToArray()
				.Select(p => new
				{
					Name = p.Name,
					Price = p.Price,
					Category = p.CategoryType.ToString(),
					Clients = p.ProductsClients.Select(pc => new
					{
						Name = pc.Client.Name,
						NumberVat = pc.Client.NumberVat
					})
					.Where(pc => pc.Name.Length >= nameLength)
					.OrderBy(p => p.Name)
					.ToArray()
				})
				.Where(p => p.Clients.Count() > 0)
				.OrderByDescending(p => p.Clients.Count())
				.ThenBy(p => p.Name)
				.Take(5)
				.ToArray();

			return JsonConvert.SerializeObject(top5Products, Formatting.Indented);
		}
	}
}