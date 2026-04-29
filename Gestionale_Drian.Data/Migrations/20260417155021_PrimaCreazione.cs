using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gestionale_Drian.Data.Migrations
{
    /// <inheritdoc />
    public partial class PrimaCreazione : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articoli",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Descrizione = table.Column<string>(type: "TEXT", nullable: true),
                    QuantitaInMagazzino = table.Column<int>(type: "INTEGER", nullable: false),
                    Prezzo = table.Column<decimal>(type: "TEXT", nullable: false),
                    Categoria = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articoli", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Associati",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumeroTessera = table.Column<string>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Cognome = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Associati", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Progetti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titolo = table.Column<string>(type: "TEXT", nullable: false),
                    Descrizione = table.Column<string>(type: "TEXT", nullable: true),
                    PrezzoTotale = table.Column<decimal>(type: "TEXT", nullable: false),
                    PreventivoPdfPath = table.Column<string>(type: "TEXT", nullable: true),
                    IsCompletato = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsConsegnato = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataConsegna = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Progetti", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movimenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ArticoloId = table.Column<int>(type: "INTEGER", nullable: false),
                    Data = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Quantita = table.Column<int>(type: "INTEGER", nullable: false),
                    Causale = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movimenti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movimenti_Articoli_ArticoloId",
                        column: x => x.ArticoloId,
                        principalTable: "Articoli",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssociatoProgetto",
                columns: table => new
                {
                    MembriId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProgettiId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssociatoProgetto", x => new { x.MembriId, x.ProgettiId });
                    table.ForeignKey(
                        name: "FK_AssociatoProgetto_Associati_MembriId",
                        column: x => x.MembriId,
                        principalTable: "Associati",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssociatoProgetto_Progetti_ProgettiId",
                        column: x => x.ProgettiId,
                        principalTable: "Progetti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssociatoProgetto_ProgettiId",
                table: "AssociatoProgetto",
                column: "ProgettiId");

            migrationBuilder.CreateIndex(
                name: "IX_Movimenti_ArticoloId",
                table: "Movimenti",
                column: "ArticoloId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssociatoProgetto");

            migrationBuilder.DropTable(
                name: "Movimenti");

            migrationBuilder.DropTable(
                name: "Associati");

            migrationBuilder.DropTable(
                name: "Progetti");

            migrationBuilder.DropTable(
                name: "Articoli");
        }
    }
}
