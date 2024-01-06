using DAL;
using Domain;
using MenuSystem;
using UnoEngine;

namespace UnoConsoleUI;

public class GameController
{
    private readonly UnoGameEngine _gameEngine;
    private readonly IGameRepository _gameRepository;
    public GameController(UnoGameEngine gameEngine, IGameRepository gameRepository)
    {
        _gameEngine = gameEngine;
        _gameRepository = gameRepository;
    }
    
    public string? MainLoop()
    {
        var save = _gameEngine.GameOptions.AutoSave;
        var exit = false;
        Console.Clear();
        
        while (!_gameEngine.IsGameOver())
        {
            if (save || _gameEngine.GameOptions.AutoSave)
            {
                _gameEngine.State.PlayerDecision = EPlayerDecision.NoneYet;
                _gameRepository.SaveGame(_gameEngine.State.Id, _gameEngine.State);
                Console.WriteLine("The game was saved");
                Thread.Sleep(_gameEngine.GameOptions.GameSpeed);
            }

            if (exit)
            {
                break;
            }

            if (_gameEngine.State.DeckOfCards.Count == 0)
            {
                _gameEngine.State.DeckOfPlayedCards.Remove(_gameEngine.State.DeckOfPlayedCards.Last());
                _gameEngine.ShuffleTheDeck(_gameEngine.State.DeckOfPlayedCards);
            }

            if (_gameEngine.State.TurnState == ETurnState.Ongoing)
            {
                save = false;
                var currentPlayer = _gameEngine.State.Players[_gameEngine.State.ActivePlayerNr];

                if (_gameEngine.State.PlayerDecision != EPlayerDecision.NoneYet) continue;
                if (currentPlayer.DrawDebt != 0)
                {
                    _gameEngine.DrawACard(currentPlayer, currentPlayer.DrawDebt);
                    currentPlayer.DrawDebt = 0;
                }

                if (currentPlayer.PlayerType != EPlayerType.Ai)
                {
                    var turnmenu = new Menu("Choose action", EMenuLevel.Turn, _gameEngine.TurnChoices());
                    turnmenu.Header = ConsoleVisualization.CreateHeader(currentPlayer, _gameEngine.State);
                    turnmenu.Run();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine(ConsoleVisualization.CreateHeader(currentPlayer, _gameEngine.State));
                    _gameEngine.AiTurn(currentPlayer);
                    Thread.Sleep(_gameEngine.GameOptions.AiSpeed);
                    Console.Clear();
                }

                switch (_gameEngine.State.PlayerDecision)
                {
                    case EPlayerDecision.Play:
                    {
                        var cards =_gameEngine.State.DeckOfPlayedCards.Count;
                        var playmenu = new Menu("Choose card", EMenuLevel.Play, _gameEngine.CardChoices());
                        playmenu.Header = ConsoleVisualization.CreateHeader(currentPlayer, _gameEngine.State);
                        
                        string? cardnr;
                        cardnr = currentPlayer.PlayerType != EPlayerType.Ai ? playmenu.Run() : _gameEngine.AiCard(currentPlayer);
                        _gameEngine.PlayACard(currentPlayer, cardnr);
                        
                        if (currentPlayer.PlayerHand.Count == 0)
                        {
                            _gameEngine.State.Winner = currentPlayer;
                        }

                        if (cards < _gameEngine.State.DeckOfPlayedCards.Count)
                        {
                            currentPlayer.CanEnd = true;
                        }
                        _gameEngine.State.PlayerDecision = EPlayerDecision.NoneYet;
                        break;
                    }
                    case EPlayerDecision.Draw:
                        _gameEngine.DrawACard(currentPlayer);
                        currentPlayer.CanDraw = false;
                        _gameEngine.State.PlayerDecision = EPlayerDecision.NoneYet;
                        break;
                    case EPlayerDecision.ShoutUno:
                        _gameEngine.Uno(currentPlayer);
                        _gameEngine.State.PlayerDecision = EPlayerDecision.NoneYet;
                        break;
                    case EPlayerDecision.Catch:
                    {
                        var catchmenu = new Menu("Catch a player", EMenuLevel.Turn, _gameEngine.CatchChoices());
                        _gameEngine.CatchPlayer(currentPlayer, catchmenu.Run());
                        
                        _gameEngine.State.PlayerDecision = EPlayerDecision.NoneYet;
                        break;
                    }
                    case EPlayerDecision.EndTurn:
                        _gameEngine.Endturn();
                        _gameEngine.State.TurnState = ETurnState.Over;
                        _gameEngine.State.PlayerDecision = EPlayerDecision.NoneYet;
                        break;
                    case EPlayerDecision.Exit:
                    {
                        exit = true;
                        if (!_gameEngine.GameOptions.AutoSave && _gameEngine.SaveTheGame() == "1")
                        {
                            save = true;
                        }
                        _gameEngine.State.TurnState = ETurnState.Over;
                        break;
                    }
                }
            }
            else
            {
                _gameEngine.State.TurnState = ETurnState.Ongoing;
            }
            
        }

        if (_gameEngine.State.Winner == null) return "";
        Console.WriteLine($"Congratulations to {_gameEngine.State.Winner.NickName}!");
        Thread.Sleep(_gameEngine.GameOptions.GameSpeed * 3);
        
        return "";
    }
}

