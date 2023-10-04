using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using P01_HospitalDatabase.Data.Models;
using P01_HospitalDatabase.Data_.Common;

namespace P01_HospitalDatabase.Data
{
    public class HospitalContext : DbContext
    {
        public HospitalContext()
        {
            
        }

        public HospitalContext(DbContextOptions options)
            : base(options)
        {
            
        }

        DbSet<Diagnose> Diagnoses { get; set; }
        DbSet<Medicament> Medicaments { get; set; }
        DbSet<Patient> Patients { get; set; }
        DbSet<PatientMedicament> PatientMedicaments { get; set; }
        DbSet<Visitation> Visitations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Config.ConnectionString);
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PatientMedicament>(pm => pm
            .HasKey(pm => new { pm.PatientId, pm.MedicamentId })); 
        }
    }
}