// See https://aka.ms/new-console-template for more information

using MenuSystem;


var newGameMenu = new Menu("New Game",EMenuLevel.Second, menuItems:new List<MenuItem>()
{
    new MenuItem()
    {
        MenuLabel = "Item 1 new game",
    },
    new MenuItem()
    {
        MenuLabel = "Item 2 new game",
    },
});

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

var mainMenu = new Menu("Main menu",EMenuLevel.First, menuItems:new List<MenuItem>()
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