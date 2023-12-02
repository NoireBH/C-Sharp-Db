using Boardgames.Common;
using Boardgames.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boardgames.DataProcessor.ExportDto
{
	public class ExportBoardgamesDto
	{
		[Required]
		public string Name { get; set; } = null!;

		[Required]
		public double Rating { get; set; }

		[Required]
		public string Mechanics { get; set; } = null!;

		[Required]
		public string Category { get; set; } = null!;

	}
}
