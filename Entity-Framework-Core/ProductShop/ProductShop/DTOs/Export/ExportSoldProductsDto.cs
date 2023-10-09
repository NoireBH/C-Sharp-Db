using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductShop.DTOs.Export
{
    public class ExportSoldProductsDto
    {
        public ExportSoldProductsDto()
        {
            SoldProducts = new HashSet<Product>();
        }

        public string? UserFirstName {  get; set; }

        public string UserLastName { get; set; } = null!;

        public virtual ICollection<Product> SoldProducts { get; set; }
    }
}
