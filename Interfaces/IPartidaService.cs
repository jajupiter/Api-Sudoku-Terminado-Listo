using SudokuApi.Models;

public interface IPartidaService
{
    public List<Partida> GetAll(string userId);
    public Partida Create(string userId);
    public string ValidateSolution(int id);
    public string ValidateIndividual(int id);

    public Partida SaveState(Partida p, string estadoTablero);
    public Partida Resume(Partida partida);
}