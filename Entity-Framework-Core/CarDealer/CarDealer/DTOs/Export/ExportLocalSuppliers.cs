using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealer.DTOs.Export
{
    internal class ExportLocalSuppliers
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int PartsCount {  get; set; }
    }
}
