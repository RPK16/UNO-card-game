using System.Text;
using ConsoleApp;
using DAL;
using Domain;
using UnoEngine;
using Microsoft.EntityFrameworkCore;
using UnoConsoleUI;
Console.OutputEncoding = Encoding.UTF8;

var gameOptions = new GameOptions();
var connectionString = "DataSource=<%temppath%>uno.db;Cache=Shared";
connectionString = connectionString.Replace("<%temppath%>", Path.GetTempPath());

var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlite(connectionString)
    .EnableDetailedErrors()
    .EnableSensitiveDataLogging()
    .Options;
using var db = new AppDbContext(contextOptions);

db.Database.Migrate();

IGameRepository gameRepository = new GameRepositoryEF(db);
// state saving functionality, can be either file system based or db based. uses the same interface for both
//IGameRepository gameRepository = new GameRepositoryFileSystem();

var mainMenu = ProgramMenus.GetMainMenu(
    gameOptions: gameOptions,
    optionsMenu: ProgramMenus.GetOptionsMenu(gameOptions),
    newGameMethod: NewGame,
    LoadGame
);

mainMenu.Run();
return;

string? NewGame()
{
    var gameEngine = new UnoGameEngine(gameOptions);
    PlayerInitializer.ConfigurePlayers(gameEngine);
    
    gameEngine.InitializeFullDeck();
    gameEngine.DealCards();
    
    var gameController = new GameController(gameEngine, gameRepository);

    gameController.MainLoop();
    return null;
}

string? LoadGame()
{
    var load = ProgramMenus.GetLoadMenu(gameRepository).Run();
    if (load == "Back")
    {
        return null;
    }
    var delete = ProgramMenus.LoadDeleteMenu(gameRepository).Run();

    var saveGameList = gameRepository.GetSaveGames();
    var gameId = saveGameList[int.Parse(load!)].id;
    var gameState = gameRepository.LoadGame(gameId);

    if (delete == "1")
    {
        gameRepository.DeleteGame(gameId, gameState);
        return null;
    }
    
    var gameEngine = new UnoGameEngine(gameOptions)
    {
        State = gameState
    };
    
    var gameController = new GameController(gameEngine, gameRepository);
    gameController.MainLoop();

    return null;
}