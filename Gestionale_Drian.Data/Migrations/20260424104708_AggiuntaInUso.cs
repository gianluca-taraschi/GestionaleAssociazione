using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gestionale_Drian.Data.Migrations
{
    /// <inheritdoc />
    public partial class AggiuntaInUso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TipoOperazione",
                table: "Movimenti",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "QuantitaInUso",
                table: "Articoli",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoOperazione",
                table: "Movimenti");

            migrationBuilder.DropColumn(
                name: "QuantitaInUso",
                table: "Articoli");
        }
    }
}
