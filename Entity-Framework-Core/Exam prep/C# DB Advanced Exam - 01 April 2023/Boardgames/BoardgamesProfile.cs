namespace Boardgames
{
    using AutoMapper;
	using Boardgames.Data.Models;
	using Boardgames.DataProcessor.ExportDto;

    public class BoardgamesProfile : Profile
    {
		// DO NOT CHANGE OR RENAME THIS CLASS!
		public BoardgamesProfile()
        {
			CreateMap<Boardgame, ExportCreatorsBoardgamesDto>();

			CreateMap<Creator, ExportCreatorsDto>()
				.ForMember(dest => dest.Name, opt => opt.MapFrom(s => s.FirstName + " " + s.LastName))
				.ForMember(dest => dest.BoardgamesCount, opt =>
					opt.MapFrom(s => s.Boardgames.Count))
				.ForMember(dest => dest.Boardgames, opt =>
					opt.MapFrom(s => s.Boardgames
										.ToArray()
										.OrderBy(b => b.Name)));


		}
    }
}