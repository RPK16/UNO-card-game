namespace Domain;

public class Player
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string NickName { get; set; } = default!;
    public EPlayerType PlayerType { get; set; }

    public bool CanPlay { get; set; } = true;
    public bool CanDraw { get; set; } = true;
    public bool CanEnd { get; set; } = false;
    public int DrawDebt { get; set; }
    public bool IsSkipped { get; set; }
    public bool Uno { get; set; }

    public List<GameCard> PlayerHand { get; set; } = new List<GameCard>();
}