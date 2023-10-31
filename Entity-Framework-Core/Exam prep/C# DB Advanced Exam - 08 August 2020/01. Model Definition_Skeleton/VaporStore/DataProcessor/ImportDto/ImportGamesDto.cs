using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaporStore.Data.Models;

namespace VaporStore.DataProcessor.ImportDto
{
    public class ImportGamesDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0.0, Double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public string ReleaseDate { get; set; } = null!;

        [Required]
        public string Developer { get; set; } = null!;

        [Required]
        public string Genre { get; set; } = null!;

        public string[] Tags { get; set; }
    }
}
