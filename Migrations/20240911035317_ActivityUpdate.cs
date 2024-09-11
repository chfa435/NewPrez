using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewTiceAI.Migrations
{
    /// <inheritdoc />
    public partial class ActivityUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Note",
                table: "ActionItems",
                newName: "Notes");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "ActionItems",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "ClosedDate",
                table: "ActionItems",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OpportunityForecastCategory",
                table: "ActionItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OpportunityStage",
                table: "ActionItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OpportunityType",
                table: "ActionItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "ActionItems");

            migrationBuilder.DropColumn(
                name: "ClosedDate",
                table: "ActionItems");

            migrationBuilder.DropColumn(
                name: "OpportunityForecastCategory",
                table: "ActionItems");

            migrationBuilder.DropColumn(
                name: "OpportunityStage",
                table: "ActionItems");

            migrationBuilder.DropColumn(
                name: "OpportunityType",
                table: "ActionItems");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "ActionItems",
                newName: "Note");
        }
    }
}
