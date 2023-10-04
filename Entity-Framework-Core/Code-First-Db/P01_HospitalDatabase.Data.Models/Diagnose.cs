using P01_HospitalDatabase.Data_.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_HospitalDatabase.Data.Models;

public class Diagnose
{
    [Key]
    public int DiagnoseId { get; set; }

    [StringLength(ValidationConstants.DiagnoseNameLength)]
    public string Name { get; set; }

    [StringLength(ValidationConstants.CommentMaxLength)]
    public string Comments { get; set; }

    public int PatientId { get; set; }

    public Patient Patient { get; set; }
}
