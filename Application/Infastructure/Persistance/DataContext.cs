using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;


namespace Application.Infastructure.Persistance
{
    public class DataContext : DbContext
    {

        protected readonly IConfiguration Configuration;
        public DbSet<HpbStatistic> HpbStatistic { get; set; }
        public DbSet<HpbHospital> HpbHospital { get; set; }
        public DbSet<HpbHospitalStatus> HpbHospitalStatus { get; set; }
        
        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var ActiveDatabase = Configuration["ActiveDatabase"];

            switch (ActiveDatabase)
            {
                case "Sqlite":
                    optionsBuilder.UseSqlite(Configuration["Database:Sqlite"]);
                    break;
                default:
                    optionsBuilder.UseSqlite(Configuration["Database:Sqlite"]);
                    break;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HpbStatistic>(Configure);
            modelBuilder.Entity<HpbHospitalStatus>(Configure);
            modelBuilder.Entity<HpbHospital>(Configure);
        }
        private void Configure(EntityTypeBuilder<HpbStatistic> modelBuilder)
        {
            modelBuilder.HasKey(a => a.Id);
            modelBuilder.HasMany(h => h.HospitalStatuses).WithOne(b=>b.HpbStatistic);
        }

        private void Configure(EntityTypeBuilder<HpbHospitalStatus> modelBuilder)
        {
            modelBuilder.HasKey(a => a.Id);
            modelBuilder.HasOne(a => a.HpbStatistic).WithMany(b => b.HospitalStatuses);
        }

        private void Configure(EntityTypeBuilder<HpbHospital> modelBuilder)
        {
            modelBuilder.HasKey(a => a.Id);
            modelBuilder.HasMany(s => s.HospitalStatuses).WithOne(b => b.HpbHospital);
        }
    }
}
