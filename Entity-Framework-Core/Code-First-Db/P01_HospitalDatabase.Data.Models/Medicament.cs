using P01_HospitalDatabase.Data_.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_HospitalDatabase.Data.Models;

public class Medicament
{
    public Medicament()
    {
        Prescriptions = new HashSet<PatientMedicament>();
    }

    [Key]
    public int MedicamentId { get; set; }

    [StringLength(ValidationConstants.MedicamentNameMaxLength)]
    public string Name { get; set; }

    public virtual ICollection<PatientMedicament> Prescriptions { get; set; }
}
