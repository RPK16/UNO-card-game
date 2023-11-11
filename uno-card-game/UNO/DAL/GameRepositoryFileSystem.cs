using System.Text.Json;
using Domain;

namespace DAL;

public class GameRepositoryFileSystem : IGameRepository<string>
{
    private readonly string _filePrefix = "." + System.IO.Path.PathSeparator;
    
    public string SaveGame(object? id, GameState game)
    {
        var filename =(string?) id ?? "uno-" + DateTime.Now.ToString() + ".json";
        System.IO.File.WriteAllText(filename, JsonSerializer.Serialize(game));

        return filename;
    }

    public GameState LoadGame(string id)
    {
        return JsonSerializer.Deserialize<GameState>(
            System.IO.File.ReadAllText(_filePrefix + id)
            )!;
    }
}