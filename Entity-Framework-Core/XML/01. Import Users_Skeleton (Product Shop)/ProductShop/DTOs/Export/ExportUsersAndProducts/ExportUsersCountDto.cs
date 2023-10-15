using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProductShop.DTOs.Export.ExportUsersAndProducts
{
    [XmlType("Users")]
    public class ExportUsersCountDto
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("users")]
        public ExportUsersWithTheirSoldProductsDto[] Users { get; set; }
    }
}
