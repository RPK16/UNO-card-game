namespace Domain;

public class GameCard
{
    public ECardColor CardColor{ get; set; }
    private string CardColorToString() =>
        CardColor switch
        {
            ECardColor.Blue => "🟦 ",
            ECardColor.Green => "🟩 ",
            ECardColor.Red => "🟥 ",
            ECardColor.Yellow => "🟨 ",
            ECardColor.Black => "⬛ ",
            _ => "-"
        };

    public ECardValue CardValue { get; set; }
    
    private string CardValueToString() =>
        CardValue switch
        {
            ECardValue.Value0 => "Value 0",
            ECardValue.Value1 => "Value 1",
            ECardValue.Value2 => "Value 2",
            ECardValue.Value3 => "Value 3",
            ECardValue.Value4 => "Value 4",
            ECardValue.Value5 => "Value 5",
            ECardValue.Value6 => "Value 6",
            ECardValue.Value7 => "Value 7",
            ECardValue.Value8 => "Value 8",
            ECardValue.Value9 => "Value 9",
            ECardValue.Change => " Wild card",
            ECardValue.Draw2 => "Draw 2",
            ECardValue.Draw4 => " Wild Draw 4",
            ECardValue.Reverse => "Reverse",
            ECardValue.Blank => "Any value",
            ECardValue.Skip => "Skip",
            _ => throw new ArgumentOutOfRangeException()
        };
    public override string ToString()
    {
        return CardColorToString() + CardValueToString();
    }
}