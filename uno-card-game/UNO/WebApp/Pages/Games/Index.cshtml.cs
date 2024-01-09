using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain.Database;
using Helpers;

namespace WebApp.Pages.Games
{
    public class IndexModel : PageModel
    {
        private readonly IGameRepository _repository;

        public IndexModel(IGameRepository repository)
        {
            _repository = repository;
        }

        //public IList<(Guid id, DateTime dt)> Game { get;set; } = default!;
        public IList<Game> Game { get; set; } = default!;

        public async Task OnGetAsync()
        {
            /*
            Game = await _context.Games
                .Include(g => g.Players)
                .OrderByDescending(g => g.UpdatedAtDt)
                .ToListAsync();
                */

            var games = _repository.GetSaveGames();


            Game = new List<Game>();

            foreach (var gameinfo in games)
            {
                var gameState = _repository.LoadGame(gameinfo.id);
                Game.Add(new Game()
                {
                    Id = gameinfo.id,
                    CreatedAtDt = gameinfo.dt,
                    UpdatedAtDt = gameinfo.dt,
                    State = System.Text.Json.JsonSerializer.Serialize(gameState, JsonHelpers.JsonSerializerOptions),
                    Players = gameState.Players.Select(p => new Domain.Database.Player()
                    {
                        Id = p.Id,
                        NickName = p.NickName,
                        PlayerType = p.PlayerType,
                        GameId = gameinfo.id,
                    }).ToList(),
                });
            }
        }
    }
}