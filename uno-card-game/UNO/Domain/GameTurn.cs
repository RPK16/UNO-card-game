namespace Domain;

public class GameTurn
{
    public GameCard UpmostCard { get; set; } = new GameCard();

    public int TurnPlayerNr { get; set; } = 0;

}