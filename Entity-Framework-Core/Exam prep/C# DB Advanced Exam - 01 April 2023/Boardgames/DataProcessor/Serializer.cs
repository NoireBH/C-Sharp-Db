namespace Boardgames.DataProcessor
{
	using AutoMapper;
	using AutoMapper.QueryableExtensions;
	using Boardgames.Data;
	using Boardgames.DataProcessor.ExportDto;
	using Boardgames.Utulities;
	using Newtonsoft.Json;

	public class Serializer
	{
		private static XmlHelper xmlHelper;

		public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
		{
			xmlHelper = new XmlHelper();

			var config = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile<BoardgamesProfile>();
			});

			ExportCreatorsDto[] creatorDtos = context.Creators
				.Where(c => c.Boardgames.Any())
				.ProjectTo<ExportCreatorsDto>(config)				
				.OrderByDescending(c => c.BoardgamesCount)
				.ThenBy(c => c.Name)
				.ToArray();

			return xmlHelper.Serialize(creatorDtos, "Creators");
		}

		public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
		{
			var top5Sellers = context.Sellers
				.Where(s => s.BoardgamesSellers
					.Any(bgs => bgs.Boardgame.YearPublished >= year && bgs.Boardgame.Rating <= rating))
				.ToArray()
				.Select(s => new ExportSellersDto()
				{
					Name = s.Name,
					Website = s.Website,
					Boardgames = s.BoardgamesSellers
					.Where(bgs => bgs.Boardgame.YearPublished >= year && bgs.Boardgame.Rating <= rating)
					.ToArray()
					.OrderByDescending(bgs => bgs.Boardgame.Rating)
					.ThenBy(bgs => bgs.Boardgame.Name)
					.Select(bgs => new ExportBoardgamesDto()
					{
						Name = bgs.Boardgame.Name,
						Rating = bgs.Boardgame.Rating,
						Mechanics = bgs.Boardgame.Mechanics,
						Category = bgs.Boardgame.CategoryType.ToString()
					})
					.ToArray()
				})
				.OrderByDescending(s => s.Boardgames.Length)
				.ThenBy(s => s.Name)
				.Take(5)
				.ToArray();

			return JsonConvert.SerializeObject(top5Sellers, Formatting.Indented);

		}
	}
}