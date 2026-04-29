using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gestionale_Drian.Data.Migrations
{
    /// <inheritdoc />
    public partial class AggiuntaVociPreventivo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ScontoPercentuale",
                table: "Progetti",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "VocePreventivo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProgettoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Descrizione = table.Column<string>(type: "TEXT", nullable: false),
                    Quantita = table.Column<int>(type: "INTEGER", nullable: false),
                    PrezzoUnitario = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VocePreventivo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VocePreventivo_Progetti_ProgettoId",
                        column: x => x.ProgettoId,
                        principalTable: "Progetti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VocePreventivo_ProgettoId",
                table: "VocePreventivo",
                column: "ProgettoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VocePreventivo");

            migrationBuilder.DropColumn(
                name: "ScontoPercentuale",
                table: "Progetti");
        }
    }
}
