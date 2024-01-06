using Domain;

namespace ConsoleApp;

public static class OptionsChanger
{
    public static string? ConfigureAutoSave(GameOptions gameOptions)
    {
        gameOptions.AutoSave = !gameOptions.AutoSave;
        return null;
    }

    public static string? ConfigureGameSpeed(GameOptions gameOptions)
    {
        Console.WriteLine("Change the game speed (How long certain text appears on the screen)");
    
        Console.Write("Speed in ms (1000 - 10 000)");
        var speedStr = Console.ReadLine()?.Trim() ?? "1000";
        if (string.IsNullOrEmpty(speedStr) || !int.TryParse(speedStr, out var gamespeed))
        {
            gamespeed = 1000;
        }

        if (gamespeed >= 10000)
        {
            gamespeed = 10000;
        } else if (gamespeed <= 1000)
        {
            gamespeed = 1000;
        }

        gameOptions.GameSpeed = gamespeed;
        return null;
    }

    public static string? ConfigureAISpeed(GameOptions gameOptions)
    {
        Console.WriteLine("Change the AI speed (How long does AI take to make a decision)");

        Console.Write("Speed in ms (1000 - 10 000)");
        var speedStr = Console.ReadLine()?.Trim() ?? "1000";
        if (string.IsNullOrEmpty(speedStr) || !int.TryParse(speedStr, out var aispeed))
        {
            aispeed = 1000;
        }

        if (aispeed >= 10000)
        {
            aispeed = 10000;
        }
        else if (aispeed <= 1000)
        { 
            aispeed = 1000; 
        }
        
        gameOptions.AiSpeed = aispeed;
        return null;
    }
}