namespace Domain;

public class Player
{
    public string Nickname { get; set; } = default!;
    public EPlayerType PlayerType { get; set; }

    public bool Played { get; set; } = false;
    public bool CanDraw { get; set; } = true;

    public List<GameCard> PlayerHand { get; set; } = new List<GameCard>();
}