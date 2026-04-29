using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gestionale_Drian.Data.Migrations
{
    /// <inheritdoc />
    public partial class AggiuntaCalendario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataCreazione",
                table: "Progetti",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DataScadenza",
                table: "Progetti",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "EventiCalendario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Data = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Titolo = table.Column<string>(type: "TEXT", nullable: false),
                    ColoreTag = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventiCalendario", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventiCalendario");

            migrationBuilder.DropColumn(
                name: "DataCreazione",
                table: "Progetti");

            migrationBuilder.DropColumn(
                name: "DataScadenza",
                table: "Progetti");
        }
    }
}
