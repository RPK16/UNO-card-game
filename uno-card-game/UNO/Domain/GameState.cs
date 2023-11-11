namespace Domain;

public class GameState
{
    // Game state to save
    public List<GameCard> DeckOfCards { get; set; } = new List<GameCard>();
    public List<GameCard> DeckOfPlayedCards { get; set; } = new List<GameCard>();
    public List<Player> Players { get; set; } = new List<Player>();
    
    public int ActivePlayerNr { get; set; } = 0; // new starter player, different methods

    // Game state to save
}