using SoftJail.Data.Models.Enums;
using SoftJail.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Officer")]
    public class ImportOfficersAndPrisonersDto
    {
        [XmlElement("Name")]
        [Required]
        [StringLength(GlobalConstants.OfficerFullNameMaxLength, MinimumLength = GlobalConstants.OfficerFullNameMinLength)]
        public string FullName { get; set; } = null!;

        [XmlElement("Money")]
        [Range(typeof(decimal), GlobalConstants.OfficerSalaryMinValue, GlobalConstants.OfficerSalaryMaxValue)]
        [Required]
        public decimal Salary { get; set; }

        [XmlElement("Position")]
        [Required]
        public string Position { get; set; } = null!;

        [XmlElement("Weapon")]
        [Required]
        public string Weapon { get; set; } = null!;

        [Required]
        [XmlElement("DepartmentId")]
        public int DepartmentId { get; set; }

        [XmlArray("Prisoners")]
        public ImportPrisonerIdDto[] Prisoners { get; set; }
    }
}
