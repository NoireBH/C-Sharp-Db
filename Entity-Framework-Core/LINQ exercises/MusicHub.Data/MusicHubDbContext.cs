using Microsoft.EntityFrameworkCore;
using MusicHub.Data.Common;
using MusicHub.Data.Models;

namespace MusicHub.Data;

public class MusicHubDbContext : DbContext
{
    public MusicHubDbContext()
    {
         
    }

    public MusicHubDbContext(DbContextOptions options)
        : base(options)
    {
        
    }

   public virtual DbSet<Album> Albums { get; set; }

    public virtual DbSet<Performer> Performers { get; set; }

    public virtual DbSet<Producer> Producers { get; set; }

    public virtual DbSet<Song> Songs { get; set; }

    public virtual DbSet<SongPerformer> SongPerformers { get; set; }

    public virtual DbSet<Writer> Writers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // sets default connection string
            optionsBuilder.UseSqlServer(DbConfig.ConnectionString);
        }

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SongPerformer>(entity =>
        {
            entity.HasKey(sp => new { sp.SongId, sp.PerformerId });
        });
    }
}