using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UnoEngine;

namespace WebApp.Pages.Play;
public class Index : PageModel
{
    private readonly DAL.AppDbContext _context;

    private readonly IGameRepository _gameRepository; 
    private GameOptions gameOptions = new GameOptions();
    public UnoGameEngine Engine { get; set; } = default!;


    public Index(AppDbContext context)
    {
        _context = context;
        _gameRepository = new GameRepositoryEF(_context);
    }

    [BindProperty(SupportsGet = true)]
    public Guid GameId { get; set; }

    [BindProperty(SupportsGet = true)]
    public Guid PlayerId { get; set; }
    
    [BindProperty (SupportsGet = true)]
    public String? CardNr { get; set; }
    
    [BindProperty (SupportsGet = true)]
    public String? ColorNr { get; set; }
    

    public void OnGet()
    {
        var gameState = _gameRepository.LoadGame(GameId);

        Engine = new UnoGameEngine(gameOptions)
        {
            State = gameState
        };
    }
    
    public void OnPost()
    {
        var gameState = _gameRepository.LoadGame(GameId);

        Engine = new UnoGameEngine(gameOptions)
        {
            State = gameState
        };

        if (Engine.GetActivePlayer().Id == PlayerId)
        {
            if (Request.Form["cardnr"] != "")
            {
                var cardstr = Request.Form["cardnr"].ToString();
                Console.WriteLine(cardstr);
                
                if (Engine.GetActivePlayer().CanPlay)
                {
                    
                    if (cardstr.Contains("-"))
                    {
                        
                        Engine.PlayACard(Engine.GetActivePlayer(), (cardstr.Split("-")[1]) , cardstr.Split("-")[0]);
                    }
                    else
                    {
                        Engine.PlayACard(Engine.GetActivePlayer(), CardNr);
                    }
                }
            }
            if (Request.Form["action"] == "draw")
            {
                if (Engine.GetActivePlayer().CanDraw)
                {
                    Engine.DrawACard(Engine.GetActivePlayer());
                }
            }
            if (Request.Form["action"] == "end")
            {
                if (Engine.GetActivePlayer().CanEnd)
                {
                    Engine.Endturn();
                }
            }
            // Save the updated game state
            _gameRepository.SaveGame(GameId, Engine.State);
        }
    }
}