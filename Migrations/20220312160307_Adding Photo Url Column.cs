using Microsoft.EntityFrameworkCore.Migrations;

namespace Parking_System_API.Migrations
{
    public partial class AddingPhotoUrlColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "Participants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "Email",
                keyValue: "admin@admin.com",
                columns: new[] { "Password", "Salt" },
                values: new object[] { "FMY6mk2lkGaJSvN1B2lpWShy7swKruM/J6ytZTbd4bs=", "sdBe38zNWgWj62oPzTMtxA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "Participants");

            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "Email",
                keyValue: "admin@admin.com",
                columns: new[] { "Password", "Salt" },
                values: new object[] { "084e78ZvtTEWX47xpD3nTr/5uRAi5wkIRx7yE+7kEwA=", "3VLhBlqPaUTHS0421dksxA==" });
        }
    }
}
