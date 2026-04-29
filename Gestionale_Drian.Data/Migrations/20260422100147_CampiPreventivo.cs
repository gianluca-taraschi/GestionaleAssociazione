using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gestionale_Drian.Data.Migrations
{
    /// <inheritdoc />
    public partial class CampiPreventivo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GiorniConsegna",
                table: "Progetti",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MetodoPagamento",
                table: "Progetti",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TipologiaConsegna",
                table: "Progetti",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GiorniConsegna",
                table: "Progetti");

            migrationBuilder.DropColumn(
                name: "MetodoPagamento",
                table: "Progetti");

            migrationBuilder.DropColumn(
                name: "TipologiaConsegna",
                table: "Progetti");
        }
    }
}
