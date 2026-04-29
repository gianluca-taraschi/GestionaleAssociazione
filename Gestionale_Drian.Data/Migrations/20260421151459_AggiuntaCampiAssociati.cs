using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gestionale_Drian.Data.Migrations
{
    /// <inheritdoc />
    public partial class AggiuntaCampiAssociati : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cellulare",
                table: "Associati",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataDiNascita",
                table: "Associati",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Indirizzo",
                table: "Associati",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cellulare",
                table: "Associati");

            migrationBuilder.DropColumn(
                name: "DataDiNascita",
                table: "Associati");

            migrationBuilder.DropColumn(
                name: "Indirizzo",
                table: "Associati");
        }
    }
}
