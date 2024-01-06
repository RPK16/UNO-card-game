using System.Text;
using Domain;


namespace UnoConsoleUI;

public abstract class ConsoleVisualization
{
    public static string ShowPlayerHand(Player player)
    {
        StringBuilder hand = new StringBuilder(player.NickName + "'s cards: " + Environment.NewLine);
        var counter = 0;

        foreach (var card in player.PlayerHand)
        {
            counter++;
            hand.Append(card);

            if (!card.Equals(player.PlayerHand.Last()))
            {
                hand.Append(", ");
            }

            if (counter == 7)
            {
                hand.AppendLine();
                counter = 0;
            }
        }

        return hand.ToString();
    }


    public static string CreateHeader(Player player, GameState state)
    {
        var header =  "Card on the table: " + state.DeckOfPlayedCards.Last() + Environment.NewLine;
        header += ShowPlayerHand(player);
        
        return header;
    }
}