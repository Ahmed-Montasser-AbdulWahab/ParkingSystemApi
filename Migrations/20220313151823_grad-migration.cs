using Microsoft.EntityFrameworkCore.Migrations;

namespace Parking_System_API.Migrations
{
    public partial class gradmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "Email",
                keyValue: "admin@admin.com",
                columns: new[] { "Password", "Salt" },
                values: new object[] { "OtSxDUApdIK3aA4JhNqoLQtLjK6asGwBqOfeCQpe3As=", "+G5HN3jMww5YNzV8q4v3bg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "Email",
                keyValue: "admin@admin.com",
                columns: new[] { "Password", "Salt" },
                values: new object[] { "vfp9AsGFdXSqazDrqpwt3CbUTGL3Tm5tytFCZ0hEbn8=", "uTSU0e4sOEUjXURMHAeJ4Q==" });
        }
    }
}
