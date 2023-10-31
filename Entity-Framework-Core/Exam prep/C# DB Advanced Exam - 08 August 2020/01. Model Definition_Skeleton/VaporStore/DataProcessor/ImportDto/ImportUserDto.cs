using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaporStore.Data.Common;
using VaporStore.Data.Models;

namespace VaporStore.DataProcessor.ImportDto
{
    public class ImportUserDto
    {
        [Required]
        [StringLength(ValidationConstants.UserUserNameMaxLength, MinimumLength = ValidationConstants.UserUserNameMinLength)]
        public string Username { get; set; } = null!;

        [Required]
        [RegularExpression(ValidationConstants.UserFullNameRegex)]
        public string FullName { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        [Range(ValidationConstants.UserAgeMin, ValidationConstants.UserAgeMax)]
        public int Age { get; set; }

        public ICollection<ImportCardsDto> Cards { get; set; }
    }
}
