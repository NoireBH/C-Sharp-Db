using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventmi.Core.Models
{
    public class EventModel : EventDetailsModel
    {
        public int Id { get; set; }
    }
}
