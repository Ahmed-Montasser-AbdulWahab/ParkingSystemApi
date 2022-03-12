using Microsoft.EntityFrameworkCore.Migrations;

namespace Parking_System_API.Migrations
{
    public partial class AddingRoleEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "AbbreviationRole", "RoleName" },
                values: new object[,]
                {
                    { 1, "p", "participant" },
                    { 2, "a", "admin" },
                    { 3, "o", "operator" }
                });

            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "Email",
                keyValue: "admin@admin.com",
                columns: new[] { "Password", "Salt" },
                values: new object[] { "084e78ZvtTEWX47xpD3nTr/5uRAi5wkIRx7yE+7kEwA=", "3VLhBlqPaUTHS0421dksxA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "Email",
                keyValue: "admin@admin.com",
                columns: new[] { "Password", "Salt" },
                values: new object[] { "3KqotRtgt+Tov/S5OirJdzBSHGzMDjGmARqG/lPnjv8=", "UPr5RNdUtp/HW5w2WyOsoQ==" });
        }
    }
}
