namespace Domain;

public class GameState
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public List<GameCard> DeckOfCards { get; set; } = new List<GameCard>();
    public List<GameCard> DeckOfPlayedCards { get; set; } = new List<GameCard>();
    public List<Player> Players { get; set; } = new List<Player>();
    public Player? PreviousPlayer { get; set; }
    public Player? Winner { get; set; }
    public ETurnState TurnState { get; set; } = ETurnState.Ongoing;
    public GameCard? TempCard { get; set; }
    public int ToDraw { get; set; } = 0;
    
    public List<string> ActionLog { get; set; } = new List<string>();
    public bool Reversed { get; set; } = false;
    public bool SkipNext { get; set; } = false;
    public GameOptions? GameOptions { get; set; }
    public EPlayerDecision PlayerDecision { get; set; } = EPlayerDecision.NoneYet;
    public int ActivePlayerNr { get; set; } = 0; 
    
}