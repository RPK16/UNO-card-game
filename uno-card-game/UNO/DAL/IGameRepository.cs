using Domain;

namespace DAL;

public interface IGameRepository
{
    void SaveGame(Guid id, GameState state);
    void DeleteGame(Guid id, GameState state);
    List<(Guid id, DateTime dt)> GetSaveGames();
    GameState LoadGame(Guid id);

}