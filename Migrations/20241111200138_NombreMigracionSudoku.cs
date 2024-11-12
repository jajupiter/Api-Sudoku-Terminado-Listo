using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sudoku.Migrations
{
    /// <inheritdoc />
    public partial class NombreMigracionSudoku : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sudoku",
                columns: table => new
                {
                    SudokuID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tablero = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Solucion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sudoku", x => x.SudokuID);
                });

            migrationBuilder.CreateTable(
                name: "Partida",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SudokuID = table.Column<int>(type: "int", nullable: false),
                    EstadoTablero = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TiempoInicio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TiempoResolucion = table.Column<TimeSpan>(type: "time", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partida", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Partida_Sudoku_SudokuID",
                        column: x => x.SudokuID,
                        principalTable: "Sudoku",
                        principalColumn: "SudokuID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Partida_SudokuID",
                table: "Partida",
                column: "SudokuID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Partida");

            migrationBuilder.DropTable(
                name: "Sudoku");
        }
    }
}
