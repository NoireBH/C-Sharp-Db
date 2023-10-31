using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaporStore.Data.Common;
using VaporStore.Data.Models.Enums;
using VaporStore.Data.Models;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.ImportDto
{
    [XmlType("Purchase")]
    public class ImportPurchaseDto
    {
        [Required]
        [XmlAttribute("title")]
        public string Title { get; set; }

        [Required]
        [XmlElement("Type")]
        public string Type { get; set; }

        [Required]
        [RegularExpression(ValidationConstants.ProductKeyRegex)]
        [XmlElement("Key")]
        public string Key { get; set; } = null!;

        [Required]
        [XmlElement("Date")]
        public string Date { get; set; } = null!;

        [Required]
        [XmlElement("Card")]
        public string Card { get; set; } = null!;
    }
}
