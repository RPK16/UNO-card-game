// See https://aka.ms/new-console-template for more information

using MenuSystem;

Console.WriteLine("Hello, World!");

var subMenu1 = new Menu("sub menu 1", menuItems:new List<MenuItem>()
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

var mainMenu = new Menu("Main menu", menuItems:new List<MenuItem>()
{
    new MenuItem()
    {
        Shortcut = "i",
        MenuLabel = "Item 1",
        MethodToRun = subMenu1.Run
    },
    new MenuItem()
    {
        Shortcut = "a",
        MenuLabel = "Item 2",
    },
});

var userChoice = mainMenu.Run();