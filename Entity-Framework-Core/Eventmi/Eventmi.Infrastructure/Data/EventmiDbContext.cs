using Eventmi.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventmi.Infrastructure.Data
{

    public class EventmiDbContext : DbContext
    {
        public EventmiDbContext()
        {

        }

        public EventmiDbContext(DbContextOptions<EventmiDbContext> options)
            : base(options)
        {

        }

        public DbSet<Event> Events { get; set; }
    }
}
