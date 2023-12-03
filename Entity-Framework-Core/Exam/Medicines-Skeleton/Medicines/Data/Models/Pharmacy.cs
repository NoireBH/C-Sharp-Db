using Medicines.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicines.Data.Models
{
	public class Pharmacy
	{
        public Pharmacy()
        {
			Medicines = new HashSet<Medicine>();
        }

        [Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(ValidationConstants.Pharmacy.NameMaxLength)]
		public string Name { get; set; } = null!;

		[Required]
		[StringLength(ValidationConstants.Pharmacy.PhoneNumberLength, MinimumLength = ValidationConstants.Pharmacy.PhoneNumberLength)]
		public string PhoneNumber { get; set; } = null!;

		[Required]
		public bool IsNonStop { get; set; }

		public ICollection<Medicine> Medicines { get; set; }
	}
}
