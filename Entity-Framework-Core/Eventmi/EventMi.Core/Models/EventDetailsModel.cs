using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventmi.Core.Models
{
    public class EventDetailsModel
    {
        [Display(Name = "Name of the event")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Field '{0}' is required!")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "FIeld '{0}' must be between {2} and {1}")]
        public string Name { get; set; } = null!;

        [Display(Name = "Date and hour started")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Field '{0}' is required!")]
        public DateTime Start { get; set; }

        [Display(Name = "EndDate and hour ended")]
        [Required]
        public DateTime End { get; set; }

        [Required]
        [StringLength(100)]
        public string Place { get; set; } = null!;
    }
}
