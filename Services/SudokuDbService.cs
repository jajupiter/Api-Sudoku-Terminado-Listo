using Microsoft.EntityFrameworkCore;
using SudokuApi.Context;
using SudokuApi.Models;

public class SudokuDbService : ISudokuService
{
    private readonly SudokuDbContext _context;
    public SudokuDbService(SudokuDbContext context)
    {
        _context = context;
    }
    public IEnumerable<Sudokus> GetAll()
    {
        return _context.Sudoku;
    }

    public Sudokus? GetById(int id)
    {
        return _context.Sudoku.Find(id);
    }
}

