using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewTiceAI.Data.Migrations
{
    /// <inheritdoc />
    public partial class AccountUpdateProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryRegion",
                table: "Accounts",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "Accounts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LegalName",
                table: "Accounts",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryRegion",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "LegalName",
                table: "Accounts");
        }
    }
}
