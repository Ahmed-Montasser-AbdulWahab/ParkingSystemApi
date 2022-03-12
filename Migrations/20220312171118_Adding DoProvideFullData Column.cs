using Microsoft.EntityFrameworkCore.Migrations;

namespace Parking_System_API.Migrations
{
    public partial class AddingDoProvideFullDataColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhotoUrl",
                table: "Participants",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: ".\\wwwroot\\images\\Anonymous.jpg",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DoProvideFullData",
                table: "Participants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "Email",
                keyValue: "admin@admin.com",
                columns: new[] { "Password", "Salt" },
                values: new object[] { "vfp9AsGFdXSqazDrqpwt3CbUTGL3Tm5tytFCZ0hEbn8=", "uTSU0e4sOEUjXURMHAeJ4Q==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoProvideFullData",
                table: "Participants");

            migrationBuilder.AlterColumn<string>(
                name: "PhotoUrl",
                table: "Participants",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: ".\\wwwroot\\images\\Anonymous.jpg");

            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "Email",
                keyValue: "admin@admin.com",
                columns: new[] { "Password", "Salt" },
                values: new object[] { "FMY6mk2lkGaJSvN1B2lpWShy7swKruM/J6ytZTbd4bs=", "sdBe38zNWgWj62oPzTMtxA==" });
        }
    }
}
