using SudokuApi.Models;

public interface ISudokuService
{
    public IEnumerable<Sudokus> GetAll();
    public Sudokus? GetById(int id);
}