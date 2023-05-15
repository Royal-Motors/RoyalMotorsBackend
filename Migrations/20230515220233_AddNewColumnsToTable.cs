using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarWebsiteBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddNewColumnsToTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "phoneNumber",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "address",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "phoneNumber",
                table: "Accounts");
        }
    }
}
