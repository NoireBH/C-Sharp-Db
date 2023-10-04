using P01_HospitalDatabase.Data_.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_HospitalDatabase.Data.Models;

public class Doctor
{
    public Doctor()
    {
        Visitations = new HashSet<Visitation>();
    }

    [Key]
    public int DoctorId { get; set; }

    [StringLength(ValidationConstants.DoctorNameMaxLength)]
    public string Name { get; set; }

    public string Specialty { get; set; }

    public virtual ICollection<Visitation> Visitations { get; set; }
}
