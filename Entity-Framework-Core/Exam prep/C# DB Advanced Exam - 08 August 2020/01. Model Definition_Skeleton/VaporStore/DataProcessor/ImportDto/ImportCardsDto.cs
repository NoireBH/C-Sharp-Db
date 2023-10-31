using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaporStore.Data.Common;
using VaporStore.Data.Models.Enums;

namespace VaporStore.DataProcessor.ImportDto
{
    public class ImportCardsDto
    {
        [Required]
        [RegularExpression(ValidationConstants.CardNumberRegex)]
        public string Number { get; set; } = null!;

        [Required]
        [RegularExpression(ValidationConstants.CvcRegex)]
        public string Cvc { get; set; } = null!;

        [Required]
        public string Type { get; set; }
    }
}
