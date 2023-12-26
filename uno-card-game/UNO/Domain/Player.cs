namespace Domain;

public class Player
{
    public string Nickname { get; set; } = default!;
    public EPlayerType PlayerType { get; set; }

    public bool CanPlay { get; set; } = true;
    public bool CanDraw { get; set; } = true;
    public int DrawDebt { get; set; } = 0;
    public bool IsSkipped { get; set; } = false;

    public List<GameCard> PlayerHand { get; set; } = new List<GameCard>();
}