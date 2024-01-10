using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using Domain;
using Domain.Database;
using UnoConsoleUI;
using UnoEngine;

namespace WebApp.Pages.Games
{
    public class CreateModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        private readonly IGameRepository _gameRepository; 
        public UnoGameEngine Engine { get; set; } = default!;


        public CreateModel(AppDbContext context, IGameRepository gameRepository)
        {
            _context = context;
            _gameRepository = new GameRepositoryEF(_context);
        }
        
        [BindProperty]
        public GameOptions GameOptions { get;set; } = default!;
        
        [BindProperty]
        [Range(0, 7, ErrorMessage = "The number of players must be between 0 and 7.")]
        public int Humans { get; set; } = 2;
        [BindProperty]
        [Range(0, 7, ErrorMessage = "The number of players must be between 0 and 7.")]
        public int Ais { get; set; } = 0;
      
        [BindProperty]
        [Range(0, 9000, ErrorMessage = "The Ai speed can be between 0 and 9000 ms.")]
        public int AiSpeed { get; set; } = 0;
       
        [BindProperty]
        [Range(0, 9000, ErrorMessage = "The game speed can be between 0 and 9000 ms.")]
        public int Gamespeed { get; set; } = 1000;
        
        [BindProperty]
        public bool Allowplay { get; set; } = false;
        
        public void OnGet()
        {
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            GameOptions.GameSpeed = Gamespeed;
            GameOptions.AiSpeed = AiSpeed;
            GameOptions.AllowPlayAfterDraw = Allowplay;
        
            Engine = new UnoGameEngine(GameOptions);
            PlayerInitializer.ConfigurePlayers(Engine, Humans, Ais);
            Engine.InitializeFullDeck();
            Engine.DealCards();
           
            _gameRepository.SaveGame(Engine.State.Id, Engine.State);
            await _context.SaveChangesAsync();
            
            return RedirectToPage("/Play/index", new { GameId = Engine.State.Id, PlayerId = Engine.State.Players[Engine.State.ActivePlayerNr].Id });
        }
    }
}
