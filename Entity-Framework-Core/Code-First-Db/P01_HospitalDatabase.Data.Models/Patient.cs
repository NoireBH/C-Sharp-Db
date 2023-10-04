using P01_HospitalDatabase.Data_.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_HospitalDatabase.Data.Models
{
    public class Patient
    {
        public Patient()
        {
            Prescriptions = new HashSet<PatientMedicament>();
        }

        [Key]
        public int PatientId { get; set; }

        [StringLength(ValidationConstants.PatientNameMaxLength)]
        public string FirstName { get; set; }

        [StringLength(ValidationConstants.PatientNameMaxLength)]
        public string LastName { get; set; }

        [StringLength(ValidationConstants.AdressMaxLength)]
        public string Address { get; set; }

        [StringLength(ValidationConstants.EmailMaxLength)]
        [Column(TypeName = "VARCHAR")]
        public string Email { get; set; }

        public bool HasInsurance {  get; set; }

        public virtual ICollection<Diagnose> Diagnoses { get; set; }

        public virtual ICollection<Visitation> Visitations { get; set; }

        public virtual ICollection<PatientMedicament> Prescriptions {  get; set; }
    }
}