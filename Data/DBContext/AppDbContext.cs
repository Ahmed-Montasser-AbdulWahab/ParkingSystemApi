using Microsoft.EntityFrameworkCore;
using Parking_System_API.Data.Entities;

namespace Parking_System_API.Data.DBContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options ): base(options)
        {

        }


        public DbSet<SystemUser> SystemUsers { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Hardware> Hardwares { get; set; }
        public DbSet<ParkingTransaction> ParkingTransactions { get; set; }
        public DbSet<Constant> Constants { get; set; }

        

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<SystemUser>().HasKey(p => new { p.Email });
            mb.Entity<Participant>().HasKey(p => new { p.ParticipantId });
            mb.Entity<Vehicle>().HasKey(p => new { p.PlateNumberId });
            mb.Entity<Hardware>().HasKey(p => new { p.HardwareId });
            mb.Entity<ParkingTransaction>().HasKey(p => new { p.ParticipantId , p.PlateNumberId, p.HardwareId, p.DateTimeTransaction});

            mb.Entity<Participant>().HasMany(p => p.Vehicles).WithMany(e => e.Participants).UsingEntity(j => j.ToTable("Participant_Vehicle"));
            mb.Entity<Participant>(entity => {
                entity.HasIndex(e => e.Email).IsUnique();
            });
            mb.Entity<Hardware>(entity => {
                entity.HasIndex(e => e.ConnectionString).IsUnique();
            });
            mb.Entity<Constant>().HasData(new Constant
            {
                ID = 1,
                ConstantName = "ForeignID",
                Value = 10000000000000,

            });
            mb.Entity<Participant>().Property(c => c.ParticipantId).ValueGeneratedNever();
            mb.Entity<Vehicle>().Property(c => c.PlateNumberId).ValueGeneratedNever();

        }

        //    mb.Entity<SystemUser>()
        //.HasData(new
        //{
        //    Email = "admin@admin.com",
        //    Name = "admin",
        //    Password = "admin",
        //    Type = true
        //});

        //    mb.Entity<SystemUser>()
        //      .HasData(new
        //      {
        //          Email = "operator@operator.com",
        //          Name = "operator",
        //          Password = "operator",
        //          Type = false
        //      });

        //}
    }
}
