using Microsoft.EntityFrameworkCore;
using Parking_System_API.Data.Entities;
using Parking_System_API.Hashing;

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
        public DbSet<Camera> Hardwares { get; set; }
        public DbSet<ParkingTransaction> ParkingTransactions { get; set; }
        public DbSet<Constant> Constants { get; set; }
        public DbSet<Role> Roles { get; set; }
        
        public DbSet<Camera> Cameras { get; set; }
        public DbSet<Terminal> Terminals { get; set; }
        public DbSet<Tariff> Tariffs   { get; set; }
        public DbSet<Gate> Gates { get; set; }

        

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Role>().HasKey(r => r.Id);
            mb.Entity<Role>().Property(r => r.Id).ValueGeneratedOnAdd();
            mb.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    RoleName = "participant",
                    AbbreviationRole = "p"
                },
                new Role
                {
                    Id=2,
                    RoleName = "admin",
                    AbbreviationRole = "a"
                },
                new Role
                {   
                    Id=3,
                    RoleName = "operator",
                    AbbreviationRole = "o"
                }


                );
            mb.Entity<SystemUser>().HasKey(p => new { p.Email });
            mb.Entity<SystemUser>().Property(p => p.IsPowerAccount).HasDefaultValue(false);

            mb.Entity<Vehicle>().HasKey(p => new { p.PlateNumberId });
            mb.Entity<Terminal>().HasKey(p => new { p.Id });
            mb.Entity<ParkingTransaction>().HasKey(p => new { p.ParticipantId , p.PlateNumberId, p.TerminalId, p.DateTimeTransaction});
            var salt = HashingClass.GenerateSalt();
            mb.Entity<SystemUser>().HasData(
                new SystemUser {
                Email = "admin@admin.com",
                Name = "Power Admin",
                Salt =  salt,
                Password = HashingClass.GenerateHashedPassword("ADMIN1234", salt),
                IsAdmin = true,
                IsPowerAccount = true
                }
                );

            mb.Entity<Terminal>().HasData(
                new Terminal
                {
                    Id = 1,
                    Service = true,
                    Direction = true
                },
                new Terminal
                {
                    Id = 2,
                    Service = true,
                    Direction = true
                }
                
                );
            mb.Entity<Gate>().HasData(
                new Gate
                {
                    Id = 1,
                    Service = true,
                    State = false,
                    TerminalId = 1
                },
                new Gate
                {
                    Id = 2,
                    Service = true,
                    State = false,
                    TerminalId = 2
                }
                ) ;
            
            mb.Entity<Participant>().Property(e => e.PhotoUrl).HasDefaultValue(".\\wwwroot\\images\\Anonymous.jpg");
            mb.Entity<Participant>().HasMany(p => p.Vehicles).WithMany(e => e.Participants).UsingEntity(j => j.ToTable("Participant_Vehicle"));
            mb.Entity<Participant>(entity => {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.NationalId).IsUnique();
            });
            mb.Entity<Camera>(entity => {
                entity.HasIndex(e => e.ConnectionString).IsUnique();
            });
            mb.Entity<Terminal>(entity => {
                entity.HasIndex(e => e.ConnectionString).IsUnique();
            });
            mb.Entity<Constant>().HasData(new Constant
            {
                ID = 1,
                ConstantName = "ForeignID",
                Value = 10000000000000,

            });
            mb.Entity<Vehicle>().Property(c => c.PlateNumberId).ValueGeneratedNever();
            mb.Entity<Terminal>().Property(c => c.Id).ValueGeneratedOnAdd();
            mb.Entity<Camera>().Property(c => c.Id).ValueGeneratedOnAdd();
            mb.Entity<Gate>().Property(c => c.Id).ValueGeneratedOnAdd();
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
