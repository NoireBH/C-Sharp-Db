﻿using Boardgames.Common;
using Boardgames.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boardgames.Data.Models
{
	public class Boardgame
	{
        public Boardgame()
        {
            BoardgamesSellers = new HashSet<BoardgameSeller>();
        }

        [Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(ValidationConstants.Boardgame.NameMaxLength)]
		public string Name { get; set; } = null!;

		[Required]
		[Range(ValidationConstants.Boardgame.RatingMinLength,ValidationConstants.Boardgame.RatingMaxLength)]
		public double Rating { get; set; }

		[Required]
		[MaxLength(ValidationConstants.Boardgame.YearPublishedMax)]
		public int YearPublished { get; set; }

		[Required]
		public CategoryType CategoryType { get; set; }

		[Required]
		public string Mechanics { get; set; } = null!;

		[Required]
		[ForeignKey(nameof(Creator))]
		public int CreatorId { get; set; }

		public Creator Creator { get; set; } = null!;

		public virtual ICollection<BoardgameSeller> BoardgamesSellers { get; set; }
	}
}
