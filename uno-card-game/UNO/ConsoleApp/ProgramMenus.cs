using DAL;
using Domain;
using UnoEngine;
using MenuSystem;

namespace ConsoleApp;

public static class ProgramMenus
{
    
    public static Menu GetOptionsMenu(GameOptions gameOptions) =>
        new Menu("Options", EMenuLevel.Second, menuItems: new List<MenuItem>() 
        {
            new MenuItem()
            {
                MenuLabelFunction = () => "Auto save - " + gameOptions.Autosaveonoff(),
                MethodToRun = () => OptionsChanger.ConfigureAutoSave(gameOptions)
            },
            new MenuItem()
            {
                MenuLabelFunction = () => "Allow play after draw - " + gameOptions.Allowplayonoff(),
                MethodToRun = () => OptionsChanger.ConfigureDraw(gameOptions)
            },
            new MenuItem()
            {
                MenuLabelFunction = () => "Game speed - " + (gameOptions.GameSpeed) + " ms",
                MethodToRun = () => OptionsChanger.ConfigureGameSpeed(gameOptions)
            },
            new MenuItem()
            {
                MenuLabelFunction = () => "AI speed - " + (gameOptions.AiSpeed) + " ms",
                MethodToRun = () => OptionsChanger.ConfigureAISpeed(gameOptions)
            },
        });
    public static Menu GetMainMenu(GameOptions gameOptions, Menu optionsMenu, Func<string?> newGameMethod, Func<string?> loadMenu) => 
        new Menu("U N O",EMenuLevel.First, menuItems:new List<MenuItem>()
        {
            new MenuItem()
            {
                MenuLabel = "New game: ",
                MenuLabelFunction = () => "Start a new game: " + gameOptions,
                MethodToRun = newGameMethod
            },
            new MenuItem()
            {
                MenuLabel = "Games",
                MethodToRun = loadMenu
            },

            new MenuItem()
            {
                MenuLabel = "Options",
                MethodToRun = optionsMenu.Run
            },
        });
    private static List<MenuItem>? LoadChoices( IGameRepository gameRepository)
    {
        var saveGameList = gameRepository.GetSaveGames();
        var saveGameListDisplay = saveGameList.Select((s, i) => (i + 1) + " - " + s).ToList();
        var loadchoices = new List<MenuItem>(); 
        
        if (saveGameListDisplay.Count == 0) return null;

        for (var gamenr = 0; gamenr < saveGameListDisplay.Count; gamenr++)
        {
            Guid gameId = saveGameList[gamenr].id;
            DateTime gameDt = saveGameList[gamenr].dt;
            
            loadchoices.Add(new MenuItem
            {
                MenuLabel = gameId + $" Last played at {gameDt}",
                ItemNr = gamenr.ToString(),
            });
        }
        return loadchoices;
    }
    public static Menu LoadDeleteMenu(IGameRepository gameRepository) =>
        new Menu("Load or delete game", EMenuLevel.Second, menuItems:new List<MenuItem>()
            {
                new MenuItem()
                {
                    MenuLabel = "Load game",
                    ItemNr = "0",
                },
                new MenuItem()
                {
                    MenuLabel = "Delete game",
                    ItemNr = "1",
                },
            }
        );
    public static Menu GetLoadMenu(IGameRepository gameRepository) =>
        new Menu("Games", EMenuLevel.Second, menuItems: LoadChoices(gameRepository) ?? new List<MenuItem>());
}