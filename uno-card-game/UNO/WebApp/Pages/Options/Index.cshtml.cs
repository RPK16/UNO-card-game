using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;
using UnoEngine;

namespace WebApp.Pages.Options

{
    public class Index : PageModel
    {
        private readonly DAL.AppDbContext _context;

        private readonly IGameRepository _gameRepository; 
        private GameOptions _gameOptions = new GameOptions();
        public UnoGameEngine Engine { get; set; } = default!;


        public Index(AppDbContext context)
        {
            _context = context;
            _gameRepository = new GameRepositoryEF(_context);
        }

        public GameOptions GameOptions { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (Engine.GameOptions != null)
            {
                GameOptions =  Engine.GameOptions;
            }
        }
    }
}