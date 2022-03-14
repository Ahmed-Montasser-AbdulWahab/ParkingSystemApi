using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Parking_System_API.Migrations
{
    public partial class InitialDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Constants",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConstantName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<long>(type: "bigint", nullable: false),
                    StringValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Constants", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Hardwares",
                columns: table => new
                {
                    HardwareId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HardwareType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConnectionString = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Service = table.Column<bool>(type: "bit", nullable: false),
                    Direction = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hardwares", x => x.HardwareId);
                });

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NationalId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: ".\\wwwroot\\images\\Anonymous.jpg"),
                    DoProvideFullData = table.Column<bool>(type: "bit", nullable: false),
                    DoProvidePhoto = table.Column<bool>(type: "bit", nullable: false),
                    DoDetected = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AbbreviationRole = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemUsers",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsPowerAccount = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemUsers", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    PlateNumberId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartSubscription = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndSubscription = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPresent = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.PlateNumberId);
                });

            migrationBuilder.CreateTable(
                name: "ParkingTransactions",
                columns: table => new
                {
                    ParticipantId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PlateNumberId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HardwareId = table.Column<int>(type: "int", nullable: false),
                    DateTimeTransaction = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Result = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingTransactions", x => new { x.ParticipantId, x.PlateNumberId, x.HardwareId, x.DateTimeTransaction });
                    table.ForeignKey(
                        name: "FK_ParkingTransactions_Hardwares_HardwareId",
                        column: x => x.HardwareId,
                        principalTable: "Hardwares",
                        principalColumn: "HardwareId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParkingTransactions_Participants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParkingTransactions_Vehicles_PlateNumberId",
                        column: x => x.PlateNumberId,
                        principalTable: "Vehicles",
                        principalColumn: "PlateNumberId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Participant_Vehicle",
                columns: table => new
                {
                    ParticipantsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VehiclesPlateNumberId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participant_Vehicle", x => new { x.ParticipantsId, x.VehiclesPlateNumberId });
                    table.ForeignKey(
                        name: "FK_Participant_Vehicle_Participants_ParticipantsId",
                        column: x => x.ParticipantsId,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participant_Vehicle_Vehicles_VehiclesPlateNumberId",
                        column: x => x.VehiclesPlateNumberId,
                        principalTable: "Vehicles",
                        principalColumn: "PlateNumberId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Constants",
                columns: new[] { "ID", "ConstantName", "StringValue", "Value" },
                values: new object[] { 1, "ForeignID", null, 10000000000000L });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "AbbreviationRole", "RoleName" },
                values: new object[,]
                {
                    { 1, "p", "participant" },
                    { 2, "a", "admin" },
                    { 3, "o", "operator" }
                });

            migrationBuilder.InsertData(
                table: "SystemUsers",
                columns: new[] { "Email", "IsAdmin", "IsPowerAccount", "Name", "Password", "Salt" },
                values: new object[] { "admin@admin.com", true, true, "Power Admin", "C4gOD/EmnwIFawHclyzZWWS2WKdWxYjkGjv3D/mx+Wg=", "/CDKAr7P0xgr22AoMNrajQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_Hardwares_ConnectionString",
                table: "Hardwares",
                column: "ConnectionString",
                unique: true,
                filter: "[ConnectionString] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingTransactions_HardwareId",
                table: "ParkingTransactions",
                column: "HardwareId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingTransactions_PlateNumberId",
                table: "ParkingTransactions",
                column: "PlateNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_Participant_Vehicle_VehiclesPlateNumberId",
                table: "Participant_Vehicle",
                column: "VehiclesPlateNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_Email",
                table: "Participants",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Participants_NationalId",
                table: "Participants",
                column: "NationalId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Constants");

            migrationBuilder.DropTable(
                name: "ParkingTransactions");

            migrationBuilder.DropTable(
                name: "Participant_Vehicle");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "SystemUsers");

            migrationBuilder.DropTable(
                name: "Hardwares");

            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropTable(
                name: "Vehicles");
        }
    }
}
