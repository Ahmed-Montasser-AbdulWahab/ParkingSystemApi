using Microsoft.EntityFrameworkCore.Migrations;

namespace Parking_System_API.Migrations
{
    public partial class NumOfVideos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumOfVideosUploaded",
                table: "Participants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "Email",
                keyValue: "admin@admin.com",
                columns: new[] { "Password", "Salt" },
                values: new object[] { "FsGolxiHcfTR7jltvqxf5ZE2fIej8XcxD99q5AAaIFk=", "gNzaHGt/NaHFeJl5kU00/Q==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumOfVideosUploaded",
                table: "Participants");

            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "Email",
                keyValue: "admin@admin.com",
                columns: new[] { "Password", "Salt" },
                values: new object[] { "C4gOD/EmnwIFawHclyzZWWS2WKdWxYjkGjv3D/mx+Wg=", "/CDKAr7P0xgr22AoMNrajQ==" });
        }
    }
}
