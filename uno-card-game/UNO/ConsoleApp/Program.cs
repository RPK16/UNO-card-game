// See https://aka.ms/new-console-template for more information

using MenuSystem;

Console.WriteLine("Hello, World!");

var mainMenu = new Menu("Main menu", menuItems:new List<MenuItem>()
{
    new MenuItem()
    {
        Shortcut = "i",
        MenuLabel = "Item 1",
    },
    new MenuItem()
    {
        Shortcut = "a",
        MenuLabel = "Item 2",
    },
});

var userChoice = mainMenu.Run();