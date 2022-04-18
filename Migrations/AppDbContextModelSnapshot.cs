﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Parking_System_API.Data.DBContext;

namespace Parking_System_API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.14")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Parking_System_API.Data.Entities.Camera", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CameraType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConnectionString")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("Service")
                        .HasColumnType("bit");

                    b.Property<int>("TerminalId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ConnectionString")
                        .IsUnique()
                        .HasFilter("[ConnectionString] IS NOT NULL");

                    b.HasIndex("TerminalId");

                    b.ToTable("Camera");
                });

            modelBuilder.Entity("Parking_System_API.Data.Entities.Constant", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConstantName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StringValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Value")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.ToTable("Constants");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            ConstantName = "ForeignID",
                            Value = 10000000000000L
                        });
                });

            modelBuilder.Entity("Parking_System_API.Data.Entities.Gate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Service")
                        .HasColumnType("bit");

                    b.Property<bool>("State")
                        .HasColumnType("bit");

                    b.Property<int>("TerminalId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TerminalId");

                    b.ToTable("Gates");
                });

            modelBuilder.Entity("Parking_System_API.Data.Entities.ParkingTransaction", b =>
                {
                    b.Property<string>("ParticipantId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PlateNumberId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("TerminalId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateTimeTransaction")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Result")
                        .HasColumnType("bit");

                    b.HasKey("ParticipantId", "PlateNumberId", "TerminalId", "DateTimeTransaction");

                    b.HasIndex("PlateNumberId");

                    b.HasIndex("TerminalId");

                    b.ToTable("ParkingTransactions");
                });

            modelBuilder.Entity("Parking_System_API.Data.Entities.Participant", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("DoDetected")
                        .HasColumnType("bit");

                    b.Property<bool>("DoProvideFullData")
                        .HasColumnType("bit");

                    b.Property<bool>("DoProvidePhoto")
                        .HasColumnType("bit");

                    b.Property<bool>("DoProvideVideo")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LastUpdated")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("NationalId")
                        .HasColumnType("bigint");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoUrl")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue(".\\wwwroot\\images\\Anonymous.jpg");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("NationalId")
                        .IsUnique();

                    b.ToTable("Participants");
                });

            modelBuilder.Entity("Parking_System_API.Data.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AbbreviationRole")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AbbreviationRole = "p",
                            RoleName = "participant"
                        },
                        new
                        {
                            Id = 2,
                            AbbreviationRole = "a",
                            RoleName = "admin"
                        },
                        new
                        {
                            Id = 3,
                            AbbreviationRole = "o",
                            RoleName = "operator"
                        });
                });

            modelBuilder.Entity("Parking_System_API.Data.Entities.SystemUser", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPowerAccount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Salt")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Email");

                    b.ToTable("SystemUsers");

                    b.HasData(
                        new
                        {
                            Email = "admin@admin.com",
                            IsAdmin = true,
                            IsPowerAccount = true,
                            Name = "Power Admin",
                            Password = "RiyYISslwAGwA/GzVOV82pCqkOeCJgvvnULB1linPfI=",
                            Salt = "xfSmDVRmw5XfaU5CsEVQ+g=="
                        });
                });

            modelBuilder.Entity("Parking_System_API.Data.Entities.Tariff", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("CostUnit")
                        .HasColumnType("float");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Tariffs");
                });

            modelBuilder.Entity("Parking_System_API.Data.Entities.Terminal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConnectionString")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("Direction")
                        .HasColumnType("bit");

                    b.Property<bool>("Service")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ConnectionString")
                        .IsUnique()
                        .HasFilter("[ConnectionString] IS NOT NULL");

                    b.ToTable("Terminals");
                });

            modelBuilder.Entity("Parking_System_API.Data.Entities.Vehicle", b =>
                {
                    b.Property<string>("PlateNumberId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BrandName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Color")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EndSubscription")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPresent")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("StartSubscription")
                        .HasColumnType("datetime2");

                    b.Property<string>("SubCategory")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PlateNumberId");

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("ParticipantVehicle", b =>
                {
                    b.Property<string>("ParticipantsId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("VehiclesPlateNumberId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ParticipantsId", "VehiclesPlateNumberId");

                    b.HasIndex("VehiclesPlateNumberId");

                    b.ToTable("Participant_Vehicle");
                });

            modelBuilder.Entity("Parking_System_API.Data.Entities.Camera", b =>
                {
                    b.HasOne("Parking_System_API.Data.Entities.Terminal", "Terminal")
                        .WithMany()
                        .HasForeignKey("TerminalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Terminal");
                });

            modelBuilder.Entity("Parking_System_API.Data.Entities.Gate", b =>
                {
                    b.HasOne("Parking_System_API.Data.Entities.Terminal", "Terminal")
                        .WithMany()
                        .HasForeignKey("TerminalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Terminal");
                });

            modelBuilder.Entity("Parking_System_API.Data.Entities.ParkingTransaction", b =>
                {
                    b.HasOne("Parking_System_API.Data.Entities.Participant", "participant")
                        .WithMany("ParkingTransactions")
                        .HasForeignKey("ParticipantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Parking_System_API.Data.Entities.Vehicle", "vehicle")
                        .WithMany("ParkingTransactions")
                        .HasForeignKey("PlateNumberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Parking_System_API.Data.Entities.Terminal", "terminal")
                        .WithMany("ParkingTransactions")
                        .HasForeignKey("TerminalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("participant");

                    b.Navigation("terminal");

                    b.Navigation("vehicle");
                });

            modelBuilder.Entity("ParticipantVehicle", b =>
                {
                    b.HasOne("Parking_System_API.Data.Entities.Participant", null)
                        .WithMany()
                        .HasForeignKey("ParticipantsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Parking_System_API.Data.Entities.Vehicle", null)
                        .WithMany()
                        .HasForeignKey("VehiclesPlateNumberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Parking_System_API.Data.Entities.Participant", b =>
                {
                    b.Navigation("ParkingTransactions");
                });

            modelBuilder.Entity("Parking_System_API.Data.Entities.Terminal", b =>
                {
                    b.Navigation("ParkingTransactions");
                });

            modelBuilder.Entity("Parking_System_API.Data.Entities.Vehicle", b =>
                {
                    b.Navigation("ParkingTransactions");
                });
#pragma warning restore 612, 618
        }
    }
}
