using MenuSystem;

namespace Domain;

public class GameState
{
    // Game state to save
    public List<GameCard> DeckOfCards { get; set; } = new List<GameCard>();
    public List<GameCard> DeckOfPlayedCards { get; set; } = new List<GameCard>();
    public List<Player> Players { get; set; } = new List<Player>();
    public ETurnState TurnState { get; set; } = ETurnState.Ongoing;

    public EPlayerDecision PlayerDecision { get; set; } = EPlayerDecision.NoneYet;

    //public Player ActivePlayer { get; set; } = Players.Find(2)
    
    public int ActivePlayerNr { get; set; } = 0; // new starter player, different methods

    // Game state to save
   

    private string Decide(EPlayerDecision decision)
    {
        PlayerDecision = decision;
        return $"Decision made: {decision}";
    }

    public Menu TurnMenuDraw;
    public Menu TurnMenu;

    public GameState()
    {
        TurnMenu = new Menu("Choose action", EMenuLevel.Turn, menuItems: new List<MenuItem>()
        {
            new MenuItem()
            {
                MenuLabel = "Play a card",
                MethodToRun = () => DecideAndReturn(EPlayerDecision.Play),
            },
            new MenuItem()
            {
                MenuLabel = "Shout UNO",
                MethodToRun = () => DecideAndReturn(EPlayerDecision.ShoutUNO),
            },
            new MenuItem()
            {
                MenuLabel = "End turn",
                MethodToRun = () => DecideAndReturn(EPlayerDecision.EndTurn),
            },
        });
        
        TurnMenuDraw = new Menu("Choose action", EMenuLevel.Turn, menuItems: new List<MenuItem>()
        {
            new MenuItem()
            {
                MenuLabel = "Play a card",
                MethodToRun = () => DecideAndReturn(EPlayerDecision.Play),
            },
            new MenuItem()
            {
                MenuLabel = "Draw a card",
                MethodToRun = () => DecideAndReturn(EPlayerDecision.Draw),
            },
            new MenuItem()
            {
                MenuLabel = "Shout UNO",
                MethodToRun = () => DecideAndReturn(EPlayerDecision.ShoutUNO),
            },
            new MenuItem()
            {
                MenuLabel = "End turn",
                MethodToRun = () => DecideAndReturn(EPlayerDecision.EndTurn),
            },
        });
    }

    private string DecideAndReturn(EPlayerDecision decision)
    {
        return Decide(decision);
    }
    
}