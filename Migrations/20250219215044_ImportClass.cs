using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NewTiceAI.Migrations
{
    /// <inheritdoc />
    public partial class ImportClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImportId",
                table: "Contacts",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Imports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ImportDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RevertDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ImportComment = table.Column<string>(type: "text", nullable: true),
                    RevertReason = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imports", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_ImportId",
                table: "Contacts",
                column: "ImportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Imports_ImportId",
                table: "Contacts",
                column: "ImportId",
                principalTable: "Imports",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Imports_ImportId",
                table: "Contacts");

            migrationBuilder.DropTable(
                name: "Imports");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_ImportId",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "ImportId",
                table: "Contacts");
        }
    }
}
