namespace Boardgames.DataProcessor
{
	using System.ComponentModel.DataAnnotations;
	using System.Text;
	using Boardgames.Data;
	using Boardgames.Data.Models;
	using Boardgames.Data.Models.Enums;
	using Boardgames.DataProcessor.ImportDto;
	using Boardgames.Utulities;
	using Newtonsoft.Json;

	public class Deserializer
	{
		private static XmlHelper xmlHelper;

		private const string ErrorMessage = "Invalid data!";

		private const string SuccessfullyImportedCreator
			= "Successfully imported creator – {0} {1} with {2} boardgames.";

		private const string SuccessfullyImportedSeller
			= "Successfully imported seller - {0} with {1} boardgames.";

		public static string ImportCreators(BoardgamesContext context, string xmlString)
		{
			StringBuilder sb = new StringBuilder();
			xmlHelper = new XmlHelper();

			ImportCreatorsDto[] creatorsDtos = xmlHelper.Deserialize<ImportCreatorsDto[]>(xmlString, "Creators");

			var validCreators = new List<Creator>();

			foreach (var creatorDto in creatorsDtos)
			{
				if (!IsValid(creatorDto))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var newCreator = new Creator()
				{
					FirstName = creatorDto.FirstName,
					LastName = creatorDto.LastName,
				};

				foreach (var boardGameDto in creatorDto.Boardgames)
				{
					if (!IsValid(boardGameDto))
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					var newBoardgame = new Boardgame()
					{
						Name = boardGameDto.Name,
						Rating = boardGameDto.Rating,
						YearPublished = boardGameDto.YearPublished,
						CategoryType = (CategoryType)boardGameDto.CategoryType,
						Mechanics = boardGameDto.Mechanics
					};

					newCreator.Boardgames.Add(newBoardgame);
				}

				validCreators.Add(newCreator);
				sb.AppendLine(String.Format(SuccessfullyImportedCreator, newCreator.FirstName, newCreator.LastName, newCreator.Boardgames.Count));
			}

			context.Creators.AddRange(validCreators);
			context.SaveChanges();
			return sb.ToString().TrimEnd();
		}

		public static string ImportSellers(BoardgamesContext context, string jsonString)
		{
			StringBuilder sb = new StringBuilder();

			ImportSellersDto[] sellerDtos = JsonConvert.DeserializeObject<ImportSellersDto[]>(jsonString);

			var validSellers = new List<Seller>();

			foreach (var sellerDto in sellerDtos)
			{
				if (!IsValid(sellerDto))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				Seller newSeller = new Seller()
				{
					Name = sellerDto.Name,
					Address = sellerDto.Address,
					Country = sellerDto.Country,
					Website = sellerDto.Website,
				};

				foreach (int boardgameId in sellerDto.Boardgames.Distinct())
                {
                    Boardgame bg = context.Boardgames.Find(boardgameId);

                    if (bg == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    newSeller.BoardgamesSellers.Add(new BoardgameSeller()
                    {
                        Boardgame = bg
                    });
                }
                validSellers.Add(newSeller);
                sb.AppendLine(String.Format(SuccessfullyImportedSeller, newSeller.Name, newSeller.BoardgamesSellers.Count));
			}

			context.Sellers.AddRange(validSellers);
			context.SaveChanges();
			return sb.ToString().TrimEnd();

		}

		private static bool IsValid(object dto)
		{
			var validationContext = new ValidationContext(dto);
			var validationResult = new List<ValidationResult>();

			return Validator.TryValidateObject(dto, validationContext, validationResult, true);
		}

	}
}
