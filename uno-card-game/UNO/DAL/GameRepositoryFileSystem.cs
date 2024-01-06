using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text.Json;
using Domain;
using Helpers;

namespace DAL;

public class GameRepositoryFileSystem : IGameRepository
{
    private const string SaveLocation = "/Users/admin/uno_games";
    //private static readonly string SaveLocation = Path.GetTempPath();


    public void SaveGame(Guid id, GameState state)
    {
        var content = JsonSerializer.Serialize(state, JsonHelpers.JsonSerializerOptions);
        
        var fileName = Path.ChangeExtension(id.ToString(), ".json");

        if (!Path.Exists(SaveLocation))
        {
            Directory.CreateDirectory(SaveLocation);
        }

        File.WriteAllText(Path.Combine(SaveLocation, fileName), content);
    }

    public List<(Guid id, DateTime dt)> GetSaveGames()
    {
        var data = Directory.EnumerateFiles(SaveLocation);
        var res = data
            .Select(
                path => (
                    Guid.Parse(Path.GetFileNameWithoutExtension(path)),
                    File.GetLastWriteTime(path)
                )
            ).ToList();
        
        return res;

    }

    public void DeleteGame(Guid id, GameState state)
    {
        var content = JsonSerializer.Serialize(state, JsonHelpers.JsonSerializerOptions);
        
        var fileName = Path.ChangeExtension(id.ToString(), ".json");

        if (Path.Exists(SaveLocation))
        {
            File.Delete(Path.Combine(SaveLocation, fileName));
        }
    }

    public GameState LoadGame(Guid id)
    {
        var fileName = Path.ChangeExtension(id.ToString(), ".json");

        var jsonStr = File.ReadAllText(Path.Combine(SaveLocation, fileName));
        var res = JsonSerializer.Deserialize<GameState>(jsonStr, JsonHelpers.JsonSerializerOptions);
        if (res == null) throw new SerializationException($"Cannot deserialize {jsonStr}");

        return res;

    }
}