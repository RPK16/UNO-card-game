using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using UnoEngine;

namespace WebApp.Pages.Play;
public class Index : PageModel
{
    private readonly DAL.AppDbContext _context;

    private readonly IGameRepository _gameRepository; 
    private GameOptions gameOptions = new GameOptions();
    public UnoGameEngine Engine { get; set; } = default!;
    
    public List<string> ActionLog { get; set; }
    
    public Index(AppDbContext context, List<string> actionLog)
    {
        _context = context;
        ActionLog = actionLog;
        _gameRepository = new GameRepositoryEF(_context);
    }

    [BindProperty(SupportsGet = true)]
    public Guid GameId { get; set; }

    [BindProperty(SupportsGet = true)]
    public Guid PlayerId { get; set; }
    
    [BindProperty (SupportsGet = true)]
    public String? CardNr { get; set; }
    
    public void OnGet()
    {
        ActionLog = ActionLog;
        var gameState = _gameRepository.LoadGame(GameId);

        Engine = new UnoGameEngine(gameOptions)
        {
            State = gameState
        };
    }
    private void AddToActionLog(string logentry)
    {
        ActionLog.Add(logentry);
    
        // Keep only the last 5 actions
        if (ActionLog.Count > 5)
        {
            ActionLog.RemoveAt(0);
        }
    }

    public void OnPost()
    {
        var gameState = _gameRepository.LoadGame(GameId);

        Engine = new UnoGameEngine(gameOptions)
        {
            State = gameState
        };
        var logentry = "";

        if (Engine.GetActivePlayer().Id == PlayerId)
        {
            if (!string.IsNullOrWhiteSpace(Request.Form["cardnr"]))
            {
                var cardstr = Request.Form["cardnr"].ToString();

                Console.WriteLine(cardstr);
                if (Engine.GetActivePlayer().CanPlay)
                {
                    var playedcard = new GameCard();
                    if (cardstr.Contains("-"))
                    {
                        Engine.PlayACard(Engine.GetActivePlayer(), (cardstr.Split("-")[1]), cardstr.Split("-")[0]);
                        playedcard = Engine.State.DeckOfPlayedCards[^2];
                        logentry =
                            $"{Engine.GetActivePlayer().NickName} played a {playedcard} and changed the color to {Engine.State.DeckOfPlayedCards.Last().CardColor}.";
                    }
                    else
                    {
                        Engine.PlayACard(Engine.GetActivePlayer(), cardstr);
                        playedcard = Engine.State.DeckOfPlayedCards[^1];
                        logentry = $"{Engine.GetActivePlayer().NickName} played a {playedcard}.";
                    }

                    AddToActionLog(logentry);
                    logentry = "";

                    switch (playedcard.CardValue)
                    {
                        case ECardValue.Draw2:
                            logentry = $"{Engine.nextPlayer().NickName} has to draw 2 cards.";
                            break;
                        case ECardValue.Draw4:
                            logentry = $"{Engine.nextPlayer().NickName} has to draw 4 cards.";
                            break;
                        case ECardValue.Skip:
                        case ECardValue.Reverse when Engine.State.Players.Count == 2:
                            logentry = $"{Engine.nextPlayer().NickName}'s turn will be skipped.";
                            break;
                    }

                }
            }

            if (Request.Form["action"] == "draw")
            {
                if (Engine.GetActivePlayer().CanDraw)
                {
                    Engine.DrawACard(Engine.GetActivePlayer());
                    logentry = $"{Engine.GetActivePlayer().NickName} drew a card.";
                }
            }

            if (Request.Form["action"] == "end")
            {
                if (Engine.GetActivePlayer().CanEnd)
                {
                    logentry = $"{Engine.GetActivePlayer().NickName} ended turn.";
                    Engine.Endturn();
                }
            }

            if (Engine.GetActivePlayer().PlayerHand.Count > 1)
            {
                Engine.GetActivePlayer().Uno = false;
            }

            if (Request.Form["action"] == "uno")
            {
                Engine.GetActivePlayer().Uno = true;
                logentry = $"{Engine.GetActivePlayer().NickName} shouted UNO!.";
            }
            
            if (!string.IsNullOrWhiteSpace(Request.Form["catch"]))
            {
                var catchplayer = Engine.State.Players[int.Parse(Request.Form["catch"]!)];
                if (catchplayer.PlayerHand.Count == 1 && !catchplayer.Uno)
                {
                    Engine.CatchPlayer(Request.Form["catch"]);
                    logentry = $"{Engine.GetActivePlayer().NickName} caught {catchplayer.NickName}. {catchplayer.NickName} must draw 4 cards.";
                    Engine.DrawACard(catchplayer, catchplayer.DrawDebt);
                }
            }

            if (!logentry.IsNullOrEmpty())
            {
                AddToActionLog(logentry);
            }
            if (Engine.GetActivePlayer().PlayerHand.Count == 0)
            {
                Engine.State.Winner = Engine.GetActivePlayer();
            }

            // Save the updated game state
            _gameRepository.SaveGame(GameId, Engine.State);
        }
    }
}