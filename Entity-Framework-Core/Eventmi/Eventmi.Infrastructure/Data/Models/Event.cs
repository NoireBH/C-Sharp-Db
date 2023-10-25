using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventmi.Infrastructure.Data.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        public DateTime Start {  get; set; }

        [Required]
        public DateTime End { get; set; }

        [Required]
        [StringLength(100)]
        public string Place {  get; set; } = null!;


    }
}
