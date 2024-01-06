using Domain;
using UnoEngine;

namespace ConsoleApp;

public static class PlayerInitializer
{
    public static void ConfigurePlayers(UnoGameEngine gameEngine)
    {
        Console.WriteLine("Choose between 2 and 10 players");
    
        Console.Write("Humans: ");
        var hCountStr = Console.ReadLine()?.Trim() ?? "0";
        if (string.IsNullOrEmpty(hCountStr) || !int.TryParse(hCountStr, out var hCount))
        {
            hCount = 0;
        }
    
        Console.Write("AIs: ");
        var aCountStr = Console.ReadLine()?.Trim() ?? "0";
        if (string.IsNullOrEmpty(aCountStr) || !int.TryParse(aCountStr, out var aCount))
        {
            aCount = 0;
        }

        switch (aCount + hCount)
        {
            case > 10:
                Console.WriteLine($"Total player amount cannot exceed 10 players. You tried to add {aCount + hCount} players");
                Thread.Sleep(gameEngine.GameOptions.GameSpeed);
                return;
            case < 2:
                Console.WriteLine("Cant start a game with less than 2 players");
                Thread.Sleep(gameEngine.GameOptions.GameSpeed);
                return;
        }

        var players = new List<Player>();
    
        for (var i = 0; i < (aCount + hCount) ; i++)
        {
            var newPlayer = new Player
            {
                NickName = "Ai" + (i + 1),
                PlayerType = EPlayerType.Ai,
            };

            if (i < hCount)
            {
                newPlayer = new Player
                {
                    NickName = "Human" + (i + 1),
                    PlayerType = EPlayerType.Human,
                };
            }
            players.Add(newPlayer);
        }
        
        gameEngine.State.Players = players;
        }
}
