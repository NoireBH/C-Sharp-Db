using Boardgames.Common;
using Boardgames.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boardgames.DataProcessor.ExportDto
{
	public class ExportSellersDto
	{
		[Required]
		public string Name { get; set; } = null!;

		[Required]
		public string Website { get; set; } = null!;

		public ExportBoardgamesDto[] Boardgames { get; set; } = null!;
	}
}
