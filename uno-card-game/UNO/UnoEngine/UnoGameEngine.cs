using Domain;
using MenuSystem;

namespace UnoEngine;

public class UnoGameEngine
{
    public GameState State { get; set; } = new GameState();
    public GameOptions GameOptions;
    private Random Rnd { get; set; } = new Random();

    private readonly Menu? _colorMenu  = new Menu("Choose color", EMenuLevel.Turn, ChangeColor());

    private const int InitialHandSize = 7;

    public UnoGameEngine(GameOptions gameOptions)
    {
        GameOptions = gameOptions;
    }
    
    public void InitializeFullDeck()
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

    public void DealCards()
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
        if (State.Players.Count > 10 || State.Players.Count < 2)
        {
            return true;
        }
        return State.Winner != null;
    }

    public void AiTurn(Player player)
    {
        var turnchoices = new List<EPlayerDecision>();
        
        if (player.CanDraw)
        {
            turnchoices.Add(EPlayerDecision.Draw);
        }

        if (player.CanEnd)
        {
            turnchoices.Add(EPlayerDecision.EndTurn); 
        }

        if (AiCard(player) != null && player.CanPlay)
        {
            Decide(EPlayerDecision.Play);
        }
        else
        {
            Decide(turnchoices[(Rnd.Next(turnchoices.Count))]);
        }
    }

    public string? AiCard(Player player)
    {
        var tableCard = State.DeckOfPlayedCards.Last();
        var tempDeck = new List<GameCard>();
        
        while (tempDeck.Count != player.PlayerHand.Count)
        {
            var randomPositionInDeck = Rnd.Next(player.PlayerHand.Count);
            tempDeck.Add(player.PlayerHand[randomPositionInDeck]);
        }

        foreach (var card in tempDeck)
        {
            if (card.CardColor == tableCard.CardColor || card.CardValue == tableCard.CardValue || card.CardColor == ECardColor.Black)
            {
                return player.PlayerHand.IndexOf(card).ToString();
            }
        }
        return null;
    }
    
    public string DrawACard(Player player, int amount = 1)
    { 
        
        if (State.DeckOfCards.Count != 0)
        {
            GameCard drawnCard = new GameCard();
            for (int cards = 1; cards <= amount; cards++)
            {
                drawnCard = State.DeckOfCards.Last();
                player.PlayerHand.Add(drawnCard);
                State.DeckOfCards.Remove(drawnCard);
            }

            if (amount == 1)
            {
                if (drawnCard.CardValue == State.DeckOfPlayedCards.Last().CardValue || drawnCard.CardColor == State.DeckOfPlayedCards.Last().CardColor)
                {
                    player.CanPlay = true;
                }
                player.CanEnd = true;
            }
            
            Console.WriteLine($"{player.NickName} drew {amount} card(s)");
            Thread.Sleep(GameOptions.GameSpeed);
            State.ToDraw = 0;
        }
        else
        {
            Console.WriteLine("Couldn't draw a card.");
            Thread.Sleep(GameOptions.GameSpeed); 
        }
        return "draw";
    }

    public string Uno(Player player)
    {
        if (player.PlayerHand.Count == 1)
        {
            player.Uno = true;
            Console.WriteLine($"{player.NickName} shouted UNO!");
        }
        else
        {
            Console.WriteLine($"Cannot shout UNO, {player.NickName} has more than one card left.");
        }
        Thread.Sleep(GameOptions.GameSpeed); 
        return "";
    }

    public Player GetNextPlayer()
    {
        Player nextplayer;
        
        if (!State.Reversed)
        {
            if (State.ActivePlayerNr < State.Players.Count - 1)
            {
                State.ActivePlayerNr++;
                nextplayer = State.Players[State.ActivePlayerNr];
            }
            else
            {
                nextplayer = State.Players[0];
                State.ActivePlayerNr = 0;
            }
        }
        else
        {
            if (State.ActivePlayerNr > 0)
            {
                State.ActivePlayerNr--;
                nextplayer = State.Players[State.ActivePlayerNr];
            }
            else
            {
                nextplayer = State.Players.Last();
                State.ActivePlayerNr = State.Players.Count - 1;
            }
        }
        return nextplayer;
    }
    
    public string Endturn()
    {
        var currentplayer = State.Players[State.ActivePlayerNr];
        if (!currentplayer.CanEnd)
        {
            Console.WriteLine("You cannot end a turn before playing or drawing a card.");
            Thread.Sleep(GameOptions.GameSpeed); 
            return "";
        }
        var nextplayer = GetNextPlayer();
        
        nextplayer.DrawDebt += State.ToDraw;
        nextplayer.IsSkipped = State.SkipNext;
        
        if (nextplayer.IsSkipped)
        {
            nextplayer.IsSkipped = false;
            Console.WriteLine(nextplayer.NickName + "'s turn was skipped.");
            Thread.Sleep(GameOptions.GameSpeed); 
            nextplayer = GetNextPlayer();
        }
        
        Reset(currentplayer);
        State.PreviousPlayer = currentplayer;
        
        Console.WriteLine(nextplayer.NickName + "'s turn.");
        Thread.Sleep(GameOptions.GameSpeed);

        return "";
    }

    public void Reset(Player player)
    {
        player.CanPlay = true;
        player.CanDraw = true;
        player.CanEnd = false;
        player.DrawDebt = 0;
        State.ToDraw = 0;
        State.SkipNext = false;
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

    private static List<MenuItem> ChangeColor()
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
    
    public List<MenuItem> CatchChoices()
    {
        var playersToCatch = new List<MenuItem>();

        for (var playerindex = 0; playerindex <= State.Players.Count - 1; playerindex++)
        {
            if (playerindex == State.ActivePlayerNr) continue;
            var player = State.Players[playerindex];
            playersToCatch.Add(new MenuItem
            {
                MenuLabel = player.NickName,
                ItemNr = playerindex.ToString(),
            });
        }
        return playersToCatch;
    }

    public void CatchPlayer(Player player, string? playernrstring)
    {
        if (playernrstring == "Back")
        {
            return;
        }
        if (playernrstring != null)
        {
            var nr = int.Parse(playernrstring);
            var playertocatch = State.Players[nr];

            if (playertocatch.PlayerHand.Count == 1 && !playertocatch.Uno)
            {
                Console.WriteLine($"{playertocatch.NickName} was caught not shouting UNO with 1 card left.");
                playertocatch.DrawDebt = 4;
            }
            else
            {
                Console.WriteLine($"{playertocatch.NickName} cannot be caught.");
            }
        }
        Thread.Sleep(GameOptions.GameSpeed); 
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
            MethodToRun = () => Decide(EPlayerDecision.ShoutUno),
        });
        
        turnchoices.Add(new MenuItem() 
        {
            MenuLabel = "Catch",
            MethodToRun = () => Decide(EPlayerDecision.Catch),
        });
        
        turnchoices.Add(new MenuItem()
        {
            MenuLabel = "End turn",
            MethodToRun = () => Decide(EPlayerDecision.EndTurn),
        });
        
        turnchoices.Add(new MenuItem()
        {
            MenuLabel = "Exit game",
            MethodToRun = () => Decide(EPlayerDecision.Exit),
        });
        return turnchoices;
    }

    private string Decide(EPlayerDecision decision)
    {
        State.PlayerDecision = decision;
        return "";
    }
    
    public void PlayACard(Player player, string? cardnrstring,string? colorstring = null)
    {
        var tablecard = State.DeckOfPlayedCards.Last();
        
        if (State.TempCard != null)
        {
            State.DeckOfPlayedCards.Remove(State.TempCard);
        }
        
        if (cardnrstring == "Back")
        {
            return;
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

                    var color = Rnd.Next(0, 4);
                    
                    if (player.PlayerType != EPlayerType.Ai)
                    {
                        if (colorstring == null)
                        {
                            colorstring = _colorMenu?.Run();
                        }
                        color = int.Parse(colorstring!);
                    }
                    
                    State.DeckOfPlayedCards.Add(card);
                    player.PlayerHand.Remove(card);
                    
                    State.TempCard = new GameCard()
                    {
                        CardColor = (ECardColor) color,
                        CardValue = ECardValue.Blank,
                    };
                    State.DeckOfPlayedCards.Add(State.TempCard);
                    player.CanPlay = false;
                    player.CanDraw = false;
                    player.CanEnd = true;
                    
                    Console.WriteLine($"{player.NickName} played a {card}");
                    Thread.Sleep(GameOptions.GameSpeed);
                    return;
                }
                
                if (tablecard.CardColor == card.CardColor || tablecard.CardValue == card.CardValue)
                {
                    if (card.CardValue == ECardValue.Draw2)
                    {
                        State.ToDraw += 2;
                    }
                    else if (card.CardValue == ECardValue.Skip)
                    {
                        State.SkipNext = true;
                    }
                    else if (card.CardValue == ECardValue.Reverse)
                    {
                        if (State.Players.Count == 2)
                        {
                            State.SkipNext = true;
                        }
                        else
                        {
                            State.Reversed = !State.Reversed;
                        }
                    }
                    State.DeckOfPlayedCards.Add(card);
                    player.PlayerHand.Remove(card);

                    player.CanPlay = false;
                    player.CanDraw = false;
                    player.CanEnd = true;
                    
                    Console.WriteLine($"{player.NickName} played a {card}");
                    Thread.Sleep(GameOptions.GameSpeed);
                    return;
                }

                Console.WriteLine($"You cannot play {card}.");
                Thread.Sleep(GameOptions.GameSpeed);
                player.CanPlay = true;
                return;
            }  
        } else {
            Console.WriteLine("No cards to play");
            Thread.Sleep(GameOptions.GameSpeed);
            player.CanPlay = false;
        }
    }
    
    public Player GetActivePlayer()
    {
        return State.Players[State.ActivePlayerNr];
    }
    
    public Player nextPlayer()
    {
        Player nextplayer;
        
        var active = State.ActivePlayerNr;
        
        if (!State.Reversed)
        {
            if (State.ActivePlayerNr < State.Players.Count - 1)
            {
                nextplayer = State.Players[active + 1];
            }
            else
            {
                nextplayer = State.Players[0];
            }
        }
        else
        {
            if (State.ActivePlayerNr > 0)
            {
                nextplayer = State.Players[active - 1];
            }
            else
            {
                nextplayer = State.Players.Last(); 
            }
        }
        return nextplayer;
    }


    public string? SaveTheGame()
    {
        var savemenu = new Menu("Would you like to save the current game state?", EMenuLevel.Turn, new List<MenuItem>()
        {
            new MenuItem()
            {
                MenuLabel = "Yes",
                ItemNr = "1",
            },
            new MenuItem()
            {
                MenuLabel = "No",
                ItemNr = "0",
            }
        });

        return savemenu.Run();
    }
}