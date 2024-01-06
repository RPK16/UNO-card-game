using System.Text.Json;
using Domain;
using Domain.Database;
using Helpers;

namespace DAL;

public class GameRepositoryEF : IGameRepository
{
    private readonly AppDbContext _ctx;

    public GameRepositoryEF(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public void SaveGame(Guid id, GameState state)
    {
        // is it already in db?
        var game = _ctx.Games.FirstOrDefault(g => g.Id == state.Id);
        if (game == null)
        {
            game = new Game()
            {
                Id = state.Id,
                State = JsonSerializer.Serialize(state, JsonHelpers.JsonSerializerOptions),
                Players = state.Players.Select(player => new Domain.Database.Player()
                {
                    Id = player.Id,
                    NickName = player.NickName,
                    PlayerType = player.PlayerType
                }).ToList()
            };
            _ctx.Games.Add(game);
        }
        else
        {
            game.UpdatedAtDt = DateTime.Now;
            game.State = JsonSerializer.Serialize(state, JsonHelpers.JsonSerializerOptions);
        }
        _ctx.SaveChanges();
    }

    public void DeleteGame(Guid id, GameState state)
    {
        var game = _ctx.Games.FirstOrDefault(g => g.Id == state.Id);
        if (game != null)
        {
            _ctx.Games.Remove(game);
        }
        _ctx.SaveChanges();
    }

    public List<(Guid id, DateTime dt)> GetSaveGames()
    {
        return _ctx.Games
            .OrderByDescending(game => game.UpdatedAtDt)
            .ToList()
            .Select(game => (game.Id, game.UpdatedAtDt))
            .ToList();
    }

    public GameState LoadGame(Guid id)
    {
        var game = _ctx.Games.First(g => g.Id == id);
        return JsonSerializer.Deserialize<GameState>(game.State, JsonHelpers.JsonSerializerOptions)!;
    }
}