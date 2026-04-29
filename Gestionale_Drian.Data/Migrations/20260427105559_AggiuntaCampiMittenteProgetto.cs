using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gestionale_Drian.Data.Migrations
{
    /// <inheritdoc />
    public partial class AggiuntaCampiMittenteProgetto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MittenteCodiceFiscale",
                table: "Progetti",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MittenteContatti",
                table: "Progetti",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MittenteIndirizzo",
                table: "Progetti",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MittenteNome",
                table: "Progetti",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MittenteCodiceFiscale",
                table: "Progetti");

            migrationBuilder.DropColumn(
                name: "MittenteContatti",
                table: "Progetti");

            migrationBuilder.DropColumn(
                name: "MittenteIndirizzo",
                table: "Progetti");

            migrationBuilder.DropColumn(
                name: "MittenteNome",
                table: "Progetti");
        }
    }
}
