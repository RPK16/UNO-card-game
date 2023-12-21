﻿using Domain;
using MenuSystem;
using UnoEngine;



namespace UnoConsoleUI;

public class GameController<TKey>
{
    private readonly UnoGameEngine<TKey> _gameEngine;
  
    public GameController(UnoGameEngine<TKey> gameEngine)
    {
        _gameEngine = gameEngine;

    }
    
    public string? MainLoop()
    {
        while (!_gameEngine.IsGameOver())
        {
            //Console.WriteLine(_gameEngine.State.Players.ToString());
            
            if (_gameEngine.State.DeckOfCards.Count == 0)
            {
                //_gameEngine.State.DeckOfCards.Add(_gameEngine.State.DeckOfPlayedCards.Last());
                _gameEngine.State.DeckOfPlayedCards.Remove(_gameEngine.State.DeckOfPlayedCards.Last());
                _gameEngine.ShuffleTheDeck(_gameEngine.State.DeckOfPlayedCards);
            }
            if (_gameEngine.State.TurnState == ETurnState.Ongoing)
            {
                Player currentPlayer = _gameEngine.State.Players[_gameEngine.State.ActivePlayerNr];
                if (_gameEngine.State.PlayerDecision == EPlayerDecision.NoneYet)
                {
                    Console.Write($"{currentPlayer} please choose an action.");
                    
                    var turnmenu = new Menu("Your cards", EMenuLevel.Turn, _gameEngine.TurnChoices());
                    turnmenu.Header = _gameEngine.CreateHeader(currentPlayer);
                    turnmenu.Run();
                    
                    if (_gameEngine.State.PlayerDecision == EPlayerDecision.Play)
                    {
                        Console.Write($"{currentPlayer} please choose a card to play");
                        
                        var playmenu = new Menu("Your cards", EMenuLevel.Play, _gameEngine.CardChoices());
                        playmenu.Header = _gameEngine.CreateHeader(currentPlayer);
                        _gameEngine.PlayACard(currentPlayer, playmenu.Run());
                       
                        _gameEngine.State.PlayerDecision = EPlayerDecision.NoneYet;
                    } 
                    else if (_gameEngine.State.PlayerDecision == EPlayerDecision.Draw)
                    {
                        Console.Write($"{currentPlayer} drew a card");
                        _gameEngine.DrawACard(currentPlayer);
                        //Console.WriteLine(currentPlayer.PlayerHand[0]);
                        _gameEngine.ShowPlayerHand(currentPlayer);
                        _gameEngine.State.PlayerDecision = EPlayerDecision.NoneYet;
                    }
                    else if (_gameEngine.State.PlayerDecision == EPlayerDecision.EndTurn)
                    {
                        _gameEngine.State.TurnState = ETurnState.Over;
                    }
                    else if (_gameEngine.State.PlayerDecision == EPlayerDecision.ShoutUNO)
                    {
                        //validate uno
                    }
                    
                }
                
            }
            else
            {
                //next players turn
            }
            
            
            
        }

        return null;
    }
}

