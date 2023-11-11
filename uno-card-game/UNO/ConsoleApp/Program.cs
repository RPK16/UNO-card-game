// See https://aka.ms/new-console-template for more information

using DAL;
using Domain;
using UnoEngine;
using MenuSystem;

var game = new UnoGameEngine<string>(repository: new GameRepositoryFileSystem());



var newGameMenu = new Menu("New Game",EMenuLevel.Second, menuItems:new List<MenuItem>()
{
    new MenuItem()
    {
        MenuLabel = "Choose players (Humans:"+ game.CountByType(EPlayerType.Human) +", AIs:" +
                    ""+ game.CountByType(EPlayerType.Ai) +")",
        MethodToRun = SetPlayers, 
        
    },
    new MenuItem()
    {
        MenuLabel = "Item 2 new game",
    },
});

string? SetPlayers()
{
    
    Console.WriteLine("Choose between 2 and 7 players");
    
    Console.Write("Humans: ");
    var hCountStr = Console.ReadLine()?.Trim();
    var hCount = int.Parse(hCountStr);
    
    Console.Write("AIs: ");
    var aCountStr = Console.ReadLine()?.Trim();
    var aCount = int.Parse(aCountStr);
    

    List<Player> Players = new List<Player>();
    for (int i = 0; i < (aCount + hCount) ; i++)
    {
        var newPlayer = new Player()
        {
            Nickname = "Ai" + i,
            PlayerType = EPlayerType.Ai,
        };

        if (i < hCount)
        {
            newPlayer = new Player()
                {
                    Nickname = "Human" + i,
                    PlayerType = EPlayerType.Human,
                };
        }
        
        game.Player.Add(newPlayer);
    }

    return null;
}




var loadGameMenu = new Menu("Load Game",EMenuLevel.Second, menuItems:new List<MenuItem>()
{
    new MenuItem()
    {
        MenuLabel = "Item 1 load",
    },
    new MenuItem()
    {
        MenuLabel = "Item 2 load",
    },
});

var mainMenu = new Menu("U N O",EMenuLevel.First, menuItems:new List<MenuItem>()
{
    new MenuItem()
    {
        MenuLabel = "New Game",
        MethodToRun = newGameMenu.Run
    },
    new MenuItem()
    {
        MenuLabel = "Load game",
        MethodToRun = loadGameMenu.Run
    },
});

var userChoice = mainMenu.Run();