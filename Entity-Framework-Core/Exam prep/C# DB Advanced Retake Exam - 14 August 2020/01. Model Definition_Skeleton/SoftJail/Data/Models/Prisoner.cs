using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftJail.Data.Models
{
    public class Prisoner
    {
        public Prisoner()
        {
            this.Mails = new HashSet<Mail>();
            this.PrisonerOfficers = new HashSet<OfficerPrisoner>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(GlobalConstants.PrisonerFullNameMaxLength, MinimumLength = GlobalConstants.PrisonerFullNameMinLength)]
        public string FullName { get; set; } = null!;

        [Required]
        public string Nickname { get; set; } = null!;

        [Required]
        [Range(GlobalConstants.PrisonerAgeMinValue, GlobalConstants.PrisonerAgeMaxValue)]
        public int Age { get; set; }

        [Required]
        public DateTime IncarcerationDate {  get; set; }

        public DateTime? ReleaseDate {  get; set; }

        public decimal? Bail {  get; set; }

        [ForeignKey(nameof(Cell))]
        public int? CellId {  get; set; }

        public Cell Cell {  get; set; }

        public ICollection<Mail> Mails {  get; set; }

        public ICollection<OfficerPrisoner> PrisonerOfficers {  get; set; }



    }
}
