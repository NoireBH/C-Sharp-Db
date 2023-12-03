using Medicines.Common;
using Medicines.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicines.Data.Models
{
	public class Medicine
	{
        public Medicine()
        {
			PatientsMedicines = new HashSet<PatientMedicine>();
        }

        [Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(ValidationConstants.Medicine.NameMaxLength)]
		public string Name { get; set; } = null!;

		[Required]
		public decimal Price { get; set; }

		[Required]
		public Category Category { get; set; }

		[Required]
		public DateTime ProductionDate { get; set; }

		[Required]
		public DateTime ExpiryDate { get; set; }

		[Required]
		[MaxLength(ValidationConstants.Medicine.ProducerMaxLength)]
		public string Producer { get; set; } = null!;

		[Required]
		[ForeignKey(nameof(Pharmacy))]
		public int PharmacyId { get; set; }

		public Pharmacy Pharmacy { get; set; } = null!;

		public virtual ICollection<PatientMedicine> PatientsMedicines { get; set; }
	}
}
