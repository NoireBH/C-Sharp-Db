using SoftJail.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftJail.DataProcessor.ImportDto
{
    public class DepartmentCellsDto
    {

        [Required]
        [StringLength(GlobalConstants.DepartmentNameMaxLength, MinimumLength = GlobalConstants.DepartmentNameMinLength)]
        public string Name { get; set; } = null!;

        public ICollection<ImportDepartmentCellDto> Cells { get; set; }
    }
}
