using SoftJail.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportPrisonersAndMailsDto
    {
        [Required]
        [StringLength(GlobalConstants.PrisonerFullNameMaxLength, MinimumLength = GlobalConstants.PrisonerFullNameMinLength)]
        public string FullName { get; set; } = null!;

        [Required]
        [RegularExpression(GlobalConstants.PrisonerNicknameRegex)]
        public string Nickname { get; set; } = null!;

        [Range(GlobalConstants.PrisonerAgeMinValue, GlobalConstants.PrisonerAgeMaxValue)]
        public int Age { get; set; }

        [Required]
        public string IncarcerationDate { get; set; } = null!;

        public string ReleaseDate { get; set; } = null!;

        [Range(typeof(decimal), GlobalConstants.PrisonerBailMinValue, GlobalConstants.PrisonerBailMaxValue)]
        public decimal? Bail { get; set; }

        public int? CellId { get; set; }

        public ICollection<ImportMailDto> Mails { get; set; }
    }
}
