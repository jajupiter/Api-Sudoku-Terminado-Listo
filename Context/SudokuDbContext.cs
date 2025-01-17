using Microsoft.EntityFrameworkCore;
using SudokuApi.Models;


namespace SudokuApi.Context
{
    public class SudokuDbContext : DbContext
    {
        public SudokuDbContext(DbContextOptions<SudokuDbContext> options) : base(options) { }
        
        public DbSet<Sudokus> Sudoku { get; set; }

        public DbSet<Partida> Partida { get; set; }
    }
}
