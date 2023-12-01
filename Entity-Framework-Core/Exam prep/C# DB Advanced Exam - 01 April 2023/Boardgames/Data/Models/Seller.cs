using Boardgames.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boardgames.Data.Models
{
	public class Seller
	{
        public Seller()
        {
            BoardgamesSellers = new HashSet<BoardgameSeller>();
        }

        [Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(ValidationConstants.Seller.NameMaxLength)]
		public string Name { get; set; } = null!;

		[Required]
		[MaxLength(ValidationConstants.Seller.AddressMaxLength)]
		public string Address { get; set; } = null!;

		[Required]
		public string Country { get; set; } = null!;

		[Required]
		[RegularExpression(ValidationConstants.Seller.WebsiteRegex)]
		public string Website { get; set; } = null!;

		public ICollection<BoardgameSeller> BoardgamesSellers { get; set; }
	}
}
