namespace Domain;

public class GameCard
{
    public ECardColor CardColor{ get; set; }

    private string CardColorToString() =>
        CardColor switch
        {
            ECardColor.Blue => "🟦",
            ECardColor.Green => "🟩",
            ECardColor.Red => "🟥",
            ECardColor.Yellow => "🟨",
            ECardColor.Black => "⬛",
            _ => "-"
        };

    public ECardValue CardValue { get; set; }

    public override string ToString()
    {
        return CardColorToString() + CardValue.ToString();
    }
}