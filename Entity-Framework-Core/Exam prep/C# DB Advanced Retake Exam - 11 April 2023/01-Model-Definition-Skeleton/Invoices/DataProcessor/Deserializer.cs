namespace Invoices.DataProcessor
{
	using System.ComponentModel.DataAnnotations;
	using System.Globalization;
	using System.Text;
	using Invoices.Data;
	using Invoices.Data.Models;
	using Invoices.DataProcessor.ImportDto;
	using Newtonsoft.Json;
	using SoftJail.Utilities;

	public class Deserializer
	{
		private readonly XmlHelper xmlHelper;

		private const string ErrorMessage = "Invalid data!";

		private const string SuccessfullyImportedClients
			= "Successfully imported client {0}.";

		private const string SuccessfullyImportedInvoices
			= "Successfully imported invoice with number {0}.";

		private const string SuccessfullyImportedProducts
			= "Successfully imported product - {0} with {1} clients.";


		public static string ImportClients(InvoicesContext context, string xmlString)
		{
			XmlHelper xmlHelper = new XmlHelper();
			StringBuilder sb = new StringBuilder();

			var clientsDtos = xmlHelper.Deserialize<ImportClientsDto[]>(xmlString, "Clients");

			var validClients = new List<Client>();

			foreach (var clientDto in clientsDtos)
			{
				if (!IsValid(clientDto))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				Client c = new Client()
				{
					Name = clientDto.Name,
					NumberVat = clientDto.NumberVat
				};

				foreach (var addressDto in clientDto.Addresses)
				{
					if (!IsValid(addressDto))
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					Address a = new Address()
					{
						StreetName = addressDto.StreetName,
						StreetNumber = addressDto.StreetNumber,
						PostCode = addressDto.PostCode,
						City = addressDto.City,
						Country = addressDto.Country
					};

					c.Addresses.Add(a);
				}

				validClients.Add(c);
				sb.AppendLine(String.Format(SuccessfullyImportedClients, c.Name));
			}

			context.Clients.AddRange(validClients);
			context.SaveChanges();
			return sb.ToString().TrimEnd();

		}


		public static string ImportInvoices(InvoicesContext context, string jsonString)
		{
			StringBuilder sb = new StringBuilder();
			var invoicesDtos = JsonConvert.DeserializeObject<ImportInvoicesDto[]>(jsonString);

			List<Invoice> validInvoices = new List<Invoice>();

			foreach (var invoiceDto in invoicesDtos)
			{
				if (!IsValid(invoiceDto))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				if (invoiceDto.DueDate == DateTime.ParseExact("01/01/0001", "dd/MM/yyyy", CultureInfo.InvariantCulture) || invoiceDto.IssueDate == DateTime.ParseExact("01/01/0001", "dd/MM/yyyy", CultureInfo.InvariantCulture))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				Invoice i = new Invoice()
				{
					Number = invoiceDto.Number,
					IssueDate = invoiceDto.IssueDate,
					DueDate = invoiceDto.DueDate,
					CurrencyType = invoiceDto.CurrencyType,
					Amount = invoiceDto.Amount,
					ClientId = invoiceDto.ClientId
				};

				if (i.IssueDate > i.DueDate)
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				validInvoices.Add(i);
				sb.AppendLine(String.Format(SuccessfullyImportedInvoices, i.Number));
			}

			context.Invoices.AddRange(validInvoices);
			context.SaveChanges();
			return sb.ToString().TrimEnd();
		}

		public static string ImportProducts(InvoicesContext context, string jsonString)
		{
			StringBuilder sb = new StringBuilder();
			var productDtos = JsonConvert.DeserializeObject<ImportProductsDto[]>(jsonString);

			List<Product> validProducts = new List<Product>();

			foreach (var productDto in productDtos)
			{
				if (!IsValid(productDto))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				Product p = new Product()
				{
					Name = productDto.Name,
					Price = productDto.Price,
					CategoryType = productDto.CategoryType,
				};

				foreach (int clientId in productDto.Clients.Distinct())
				{
					Client c = context.Clients.Find(clientId);
					if (c == null)
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					p.ProductsClients.Add(new ProductClient()
					{
						Client = c
					});
				}
				validProducts.Add(p);
				sb.AppendLine(String.Format(SuccessfullyImportedProducts, p.Name, p.ProductsClients.Count));
			}

			context.Products.AddRange(validProducts);
			context.SaveChanges();
			return sb.ToString().TrimEnd();

		}

		public static bool IsValid(object dto)
		{
			var validationContext = new ValidationContext(dto);
			var validationResult = new List<ValidationResult>();

			return Validator.TryValidateObject(dto, validationContext, validationResult, true);
		}
	}
}
