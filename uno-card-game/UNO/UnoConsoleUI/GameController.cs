using Domain;
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
            if (_gameEngine.State.DeckOfCards.Count == 0)
            {
                //_gameEngine.State.DeckOfCards.Add(_gameEngine.State.DeckOfPlayedCards.Last());
                _gameEngine.State.DeckOfPlayedCards.Remove(_gameEngine.State.DeckOfPlayedCards.Last());
                _gameEngine.ShuffleTheDeck(_gameEngine.State.DeckOfPlayedCards);
            }
            if (_gameEngine.IsItNewMove())
            {
                Console.Write($"Player {_gameEngine.State.ActivePlayerNr + 1} please choose a card to play");
                var card = Console.ReadLine();
                _gameEngine.PlayACard(Player, card);
            }

            if (_gameEngine.State.PlayerDecision == EPlayerDecision.NoneYet)
            {
                Console.Write($"Player {_gameEngine.State.ActivePlayerNr + 1} please choose a card to play");

            }
            
        }

        return null;
    }
}

