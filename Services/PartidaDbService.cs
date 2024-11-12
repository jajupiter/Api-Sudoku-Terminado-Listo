using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SudokuApi.Context;
using SudokuApi.Models;

public class PartidaDbService : IPartidaService
{
    private readonly SudokuDbContext _context;

    public PartidaDbService(SudokuDbContext context)
    {
        _context = context;
    }
    public Partida Create(string userId)
    {
        var listaSudokus = _context.Sudoku.ToList();
        var random = new Random();
        if(listaSudokus.Count() == 0)
        {
            return null;
        } 
        var sudokuSeleccionado = listaSudokus[random.Next(listaSudokus.Count)];
        Partida partida = new()
        {
            SudokuID = sudokuSeleccionado.SudokuID,
            EstadoTablero = sudokuSeleccionado.Tablero,
            TiempoInicio = DateTime.UtcNow,
            TiempoResolucion = TimeSpan.Zero,
            UserId = userId  // Asignar el ID del usuario logueado
        };

        _context.Partida.Add(partida);
        _context.SaveChanges();
        return partida;
    }

    public List<Partida> GetAll(string userId)
    {
        var partidas = _context.Partida
            .Where(p => p.UserId == userId)
            .ToList();
        return partidas;
    }

    public Partida Resume(Partida p)
    {
        p.TiempoInicio = DateTime.UtcNow;
        _context.Partida.Update(p);
        _context.SaveChanges();
        return p;
    }

    public Partida SaveState(Partida p, string estadoTablero)
    {
        var tiempoActual = DateTime.UtcNow;
        p.TiempoResolucion += tiempoActual - p.TiempoInicio;
        // Reinicia el tiempo de inicio a null hasta que se vuelva a reanudar
        p.TiempoInicio = null;
        // Guarda el estado del tablero
        p.EstadoTablero = estadoTablero;
        _context.Partida.Update(p);
        _context.SaveChanges();
        return p;
    }

    public string ValidateSolution(int id)
    {
        var partida = _context.Partida.Find(id);
        var sudoku = _context.Sudoku.Find(partida.SudokuID);
            if (sudoku == null)
            {
                return "1";
            }

            if (partida.TiempoInicio != null)
            {
                var tiempoActual = DateTime.UtcNow;
                partida.TiempoResolucion += tiempoActual - partida.TiempoInicio;
                partida.TiempoInicio = null;
            }

            _context.Partida.Update(partida);
            _context.SaveChanges();
        
            if (partida.EstadoTablero == sudoku.Solucion)
            {
                // La solución es correcta, elimina la partida de la base de datos
                _context.Partida.Remove(partida);
                _context.SaveChanges();
                return "2";
            }
            else return null;
    }

    public string ValidateIndividual(int id)
    {
        var partida = _context.Partida.Find(id);
        var sudoku = _context.Sudoku.Find(partida.SudokuID);
        char[] arraySol = partida.EstadoTablero.ToCharArray();
        List<int> posicionesIncorrectas = new List<int>();

        for (int i = 0; i < arraySol.Length; i++)
        {
            if (arraySol[i] != '0' && arraySol[i] != sudoku.Solucion[i])
            {
                posicionesIncorrectas.Add(i); 
                arraySol[i] = 'X'; 
            }
        }
        return "Solucion incorrecta: " + new string(arraySol);
    }
    
}
