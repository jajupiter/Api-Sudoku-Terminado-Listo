using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SudokuApi.Models
{
    public class Partida
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Sudoku")]
        public int SudokuID { get; set; }
        public Sudokus Sudoku { get; set; }  // Propiedad de navegación a Sudokus

        public string? EstadoTablero { get; set; }
        public DateTime? TiempoInicio { get; set; }
        public TimeSpan? TiempoResolucion { get; set; }

        public string UserId { get; set; }  // Esto es solo un campo, no una clave foránea
    }
}
