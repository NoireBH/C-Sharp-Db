using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftJail.Data.Models
{
    public class Department
    {
        public Department()
        {
            this.Cells = new HashSet<Cell>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(GlobalConstants.DepartmentNameMaxLength, MinimumLength = GlobalConstants.DepartmentNameMinLength)]
        public string Name { get; set; } = null!;

        public ICollection<Cell> Cells { get; set; }
    }
}
