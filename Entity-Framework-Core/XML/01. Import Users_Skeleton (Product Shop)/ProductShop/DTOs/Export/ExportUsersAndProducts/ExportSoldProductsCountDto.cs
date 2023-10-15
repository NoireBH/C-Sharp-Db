using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProductShop.DTOs.Export.ExportUsersAndProducts
{
    [XmlType("SoldProducts")]
    public class ExportSoldProductsCountDto
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("products")]
        public ExportSoldProductDto[] Products { get; set; }
    }
}
