// See https://aka.ms/new-console-template for more information

using MenuSystem;

Console.WriteLine("Hello, World!");

var subMenu1 = new Menu("sub menu 1",EMenuLevel.Second, menuItems:new List<MenuItem>()
{
    new MenuItem()
    {
        MenuLabel = "Item 1 sub",
    },
    new MenuItem()
    {
        MenuLabel = "Item 2 sub",
    },
});

var mainMenu = new Menu("Main menu",EMenuLevel.First, menuItems:new List<MenuItem>()
{
    new MenuItem()
    {
        MenuLabel = "Item 1",
        MethodToRun = subMenu1.Run
    },
    new MenuItem()
    {
        MenuLabel = "Item 2",
        //MethodToRun = subMenu1.Run
    },
});

var userChoice = mainMenu.Run();