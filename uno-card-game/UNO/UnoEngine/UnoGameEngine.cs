using System.Diagnostics;
using System.Reflection.Emit;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using DAL;
using Domain;
using MenuSystem;

namespace UnoEngine;

public class UnoGameEngine<TKey>
{
    public IGameRepository<TKey> GameRepository { get; set; }
    public GameState State { get; set; } = new GameState();
    private Random Rnd { get; set; } = new Random();
    
    public Menu? PlayMenu;
    public Menu? TurnMenu;
    public Menu? ColorMenu;
    
    private const int InitialHandSize = 7;
    //public GameCard? StartCard { get; set; }

    public UnoGameEngine(IGameRepository<TKey> repository)
    {
        InitializePlayers();
        InitializeFullDeck();
        DealCards();
        GameRepository = repository;
        
        //PlayMenu = new Menu("Choose a card to play", EMenuLevel.Play, CardChoices());
        //TurnMenu = new Menu("Choose action", EMenuLevel.Turn, TurnChoices());
        ColorMenu = new Menu("Choose color", EMenuLevel.Turn, ChangeColor());
    }

    public void NextMove()
    {
        
    }

   
    public void SaveGame()
    {
        GameRepository.SaveGame(null, State);
    }
    
    public void InitializePlayers()
    {
        State.Players = new List<Player>()
        {
            new Player()
            {
                Nickname = "Human1",
                PlayerType = EPlayerType.Human
            },
            new Player()
            {
                Nickname = "Ai1",
                PlayerType = EPlayerType.Ai
            },
    
        };
    }
    
    private void InitializeFullDeck()
    {
        for (int cardColor = 0; cardColor <= (int) ECardColor.Green; cardColor++)
        {
            State.DeckOfCards.Add(new GameCard()
            {
                CardColor = (ECardColor) cardColor,
                CardValue = 0,
            });
            
            for (int cardValue = 1; cardValue <= (int) ECardValue.Value9; cardValue++)
            {
                State.DeckOfCards.Add(new GameCard()
                {
                    CardColor = (ECardColor) cardColor,
                    CardValue = (ECardValue) cardValue,
                });
                
                State.DeckOfCards.Add(new GameCard()
                {
                    CardColor = (ECardColor) cardColor,
                    CardValue = (ECardValue) cardValue,
                });
            }
            for (int cardValue = (int) ECardValue.Skip; cardValue <= (int) ECardValue.Draw2; cardValue++)
            {
                State.DeckOfCards.Add(new GameCard()
                {
                    CardColor = (ECardColor) cardColor,
                    CardValue = (ECardValue) cardValue,
                });
                
                State.DeckOfCards.Add(new GameCard()
                {
                    CardColor = (ECardColor) cardColor,
                    CardValue = (ECardValue) cardValue,
                });
            }
        }
        

        for (int i = 0; i < 4; i++)
        {
            State.DeckOfCards.Add(new GameCard()
            {
                CardColor = ECardColor.Black,
                CardValue = ECardValue.Change,
            });
            State.DeckOfCards.Add(new GameCard()
            {
                CardColor = ECardColor.Black,
                CardValue = ECardValue.Draw4,
            });
        }
        State.DeckOfCards = ShuffleTheDeck(State.DeckOfCards);
    }

    public List<GameCard> ShuffleTheDeck(List<GameCard> deck)
    {
        var randomDeck = new List<GameCard>();
        while (deck.Count > 0)
        {
            var randomPositionInDeck = Rnd.Next(deck.Count);
            randomDeck.Add(deck[randomPositionInDeck]);
            deck.RemoveAt(randomPositionInDeck);
        }
        return randomDeck;
    }

    private void DealCards()
    {
        foreach (var player in State.Players)
        {
            for (int cardNr = 0; cardNr < InitialHandSize; cardNr++)
            {
                player.PlayerHand.Add(State.DeckOfCards.Last());
                State.DeckOfCards.Remove(State.DeckOfCards.Last());
            }
        }

        while (!(State.DeckOfCards.Last().CardValue <= ECardValue.Value9))
        {
            State.DeckOfCards.Remove(State.DeckOfCards.Last());
            State.DeckOfCards.Add(State.DeckOfCards.Last());
        }
        State.DeckOfPlayedCards.Add(State.DeckOfCards.Last());
        State.DeckOfCards.Remove(State.DeckOfCards.Last());
    }

    public bool IsGameOver()
    {
        return false;
    }

    public bool IsItNewMove()
    {
        //if (State.ActivePlayerNr != State)
        //{
            return true;
       // }
       // return false;
    }
    //
    public string DrawACard(Player player)
    {
        if (State.DeckOfCards.Count != 0)
        {
            int count = 0;
            
            for (int cards = 0; cards <= player.DrawDebt; cards++)
            {
                count++;
                GameCard drawnCard = State.DeckOfCards.Last();
                player.PlayerHand.Add(drawnCard);
                State.DeckOfCards.Remove(drawnCard);
            }
            
            Console.WriteLine($"{player.Nickname} drew {count} card(s)");
            Thread.Sleep(2000);
        }
        else
        {
            Console.WriteLine("Couldn't draw a card.");
            Thread.Sleep(2000);   
        }
        return "";
    }
    
    public string Endturn()
    {
        
        var nextplayer = State.Players[0];
        
        if (State.ActivePlayerNr < State.Players.Count - 1)
        {
            State.ActivePlayerNr++;
            nextplayer = State.Players[State.ActivePlayerNr - 1];
        }
        else
        {
            nextplayer = State.Players[0];
            State.ActivePlayerNr = 0;
        }

        if (nextplayer.IsSkipped)
        {
            Console.WriteLine(nextplayer.Nickname + "'s turn was skipped.");
            Thread.Sleep(1000);
            State.ActivePlayerNr++;
            
        }
        
        Console.WriteLine(State.Players[State.ActivePlayerNr].Nickname + "'s turn.");
        Thread.Sleep(1000);

        return "";
        
    }

    public string ShowPlayerHand(Player player)
    {
        string hand = player.Nickname + "'s cards: ";
        foreach (var card in player.PlayerHand)
        {
            hand += card;
            if (!card.Equals(player.PlayerHand.Last()))
            {
                hand += ", ";
            }
        }

        return hand;
    }

    public string CreateHeader(Player player)
    {
        string header =  "Card on the table: " + State.DeckOfPlayedCards.Last().ToString() + System.Environment.NewLine;
        //header += State.Players[State.ActivePlayerNr].Nickname + "'s turn. " + System.Environment.NewLine;
        header += ShowPlayerHand(player);
        
        return header;
    }


    // public void ChooseAction()
    // {
    //     var newGameMenu = new Menu("New Game", EMenuLevel.Second, menuItems: new List<MenuItem>());
    // }
    private string ReturnNumber(int number)
    {
        return number.ToString();
    }

    public List<MenuItem> CardChoices()
    {
        var player = State.Players[State.ActivePlayerNr];
        var cardChoices = new List<MenuItem>();

        for (var cardnr = 0; cardnr < player.PlayerHand.Count; cardnr++)
        {
            var card = player.PlayerHand[cardnr];
            cardChoices.Add(new MenuItem
            {
                MenuLabel = card.ToString(),
                ItemNr = cardnr.ToString()
            });
        }
        
        return cardChoices;
    }

    public List<MenuItem> ChangeColor()
    {
        var colorChoices = new List<MenuItem>();

        for (int cardColor = 0; cardColor <= (int) ECardColor.Black; cardColor++)
        {
            var label = (ECardColor)cardColor;
            colorChoices.Add(new MenuItem
            {
                MenuLabel = label.ToString(),
                ItemNr = cardColor.ToString()
            });
        }
        
        return colorChoices;
    }

    public List<MenuItem> TurnChoices()
    {
        var player = State.Players[State.ActivePlayerNr];
        var turnchoices = new List<MenuItem>();
            
        if (player.CanPlay)
        {
            var play = new MenuItem()
            {
                MenuLabel = "Play card(s)",
                MethodToRun = () => Decide(EPlayerDecision.Play),
            };
            turnchoices.Add(play);
        }

        if (player.CanDraw)
        {
            var draw = new MenuItem()
            {
                MenuLabel = "Draw card(s)",
                MethodToRun = () => Decide(EPlayerDecision.Draw),
            };
            turnchoices.Add(draw);
        }
        
        turnchoices.Add( new MenuItem()
            {
                MenuLabel = "Shout UNO",
                MethodToRun = () => Decide(EPlayerDecision.ShoutUNO),
            });
        
        turnchoices.Add(new MenuItem()
        {
            MenuLabel = "End turn",
            MethodToRun = () => Decide(EPlayerDecision.EndTurn),
        });
        
        return turnchoices;
    }
    
    private string Decide(EPlayerDecision decision)
    {
        State.PlayerDecision = decision;
        return "";
    }
    
    public string PlayACard(Player player, string? cardnrstring)
    {
        
        if (cardnrstring == "Back")
        {
            return "";
        }
        if (cardnrstring != null)
        {
            var nr = int.Parse(cardnrstring);
            
            if (player.PlayerHand.Count >= nr)
            {
                var card = player.PlayerHand[nr];
                
                if (card.CardColor == ECardColor.Black)
                {
                    if (card.CardValue == ECardValue.Draw4)
                    {
                        State.ToDraw += 4;
                    }
                    var colorstring = ColorMenu?.Run();
                    var color = int.Parse(colorstring);
                    
                    State.DeckOfPlayedCards.Add(new GameCard()
                    {
                        CardColor = (ECardColor) color,
                        CardValue = ECardValue.Blank,
                    });
                }
                
                else if (State.DeckOfPlayedCards.Last().CardColor == card.CardColor ||
                    State.DeckOfPlayedCards.Last().CardValue == card.CardValue)
                {
                    if (card.CardValue == ECardValue.Draw2)
                    {
                        State.ToDraw += 2;
                    }
                    State.DeckOfPlayedCards.Add(card);
                    player.PlayerHand.Remove(card);

                    player.CanPlay = false;
                    player.CanDraw = false;
                    
                    return "";
                }
                else
                {
                    Console.WriteLine($"You cannot play {card}.");
                    Thread.Sleep(2000);
                    player.CanPlay = true;
                }
            }  
        } else {
            Console.WriteLine("No cards to play");
            Thread.Sleep(2000);
            player.CanPlay = true;
        }
        return "";
    }

    public int CountByType(EPlayerType pType)
    {
        return State.Players.Count(player => pType == player.PlayerType);
    }
}