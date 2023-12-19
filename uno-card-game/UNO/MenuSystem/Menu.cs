namespace MenuSystem;

public class Menu
{
    public string? Title { get; set; }

    //public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    public Dictionary<int, MenuItem> MenuItems { get; set; } = new Dictionary<int, MenuItem>();
    public int Option;

    private const string MenuSeparator = "======================";

    public Menu(string? title, EMenuLevel menuLevel, List<MenuItem> menuItems)
    {
        var counter = 0;
        Title = title;
        foreach (var menuItem in menuItems)
        {
            counter++;
            MenuItems[counter] = menuItem;
        }

        counter++;

        switch (menuLevel)
        {
            case EMenuLevel.Turn:
                MenuItems[counter] = new MenuItem() { MenuLabel = "Back" };
                break;
            case EMenuLevel.First:
                MenuItems[counter] = new MenuItem() { MenuLabel = "Exit" };
                break;
            case EMenuLevel.Second:
                MenuItems[counter] = new MenuItem() { MenuLabel = "Back" };
                break;
            case EMenuLevel.Other:
                MenuItems[counter] = new MenuItem() { MenuLabel = "Back" };
                MenuItems[counter + 1] = new MenuItem() { MenuLabel = "Return to main menu" };
                break;
        }
    }

    public bool Back(int x = 1)
    {
        if (x <= 0)
        {
            return false;
        }
        return true;
    }


    private static void ItemWrite(MenuItem item)
    {
        var color = "";
        if (item.IsSelected)
        {
            color = " \u001b[32m";
        }

        if (item.MenuLabelFunction != null)
        {
            Console.WriteLine(color + item.MenuLabelFunction() + "\u001b[0m");
        }
        else
        {
            Console.WriteLine(color + item.MenuLabel + "\u001b[0m");
        }
    }

    private void Draw()
    {
        if (!string.IsNullOrWhiteSpace(Title))
        {
            Console.WriteLine(Title);
            Console.WriteLine(MenuSeparator);
            Console.WriteLine("Navigate the menu using arrow keys. Make your choice with Enter key.");
            Console.WriteLine();
        }


        foreach (var menuItem in MenuItems)
        {
            menuItem.Value.IsSelected = MenuItems.ElementAt(Option).Key == menuItem.Key;
            //Console.Write(menuItem.Key);
            ItemWrite(menuItem.Value);
        }

    }
    public string? Run()
    {
        var closeMenu = false;
        MenuItem? userChoice = null;
        Console.Clear();
        ConsoleKeyInfo key;
        String[] PlayActions = { "Play a card", "Draw a card" };

        do
        {
            Console.Clear();
            Draw();
            

            key = Console.ReadKey();

            switch (key.Key)
            {
                case ConsoleKey.DownArrow:
                    Option = (Option + 1) % (MenuItems.Count);
                    break;

                case ConsoleKey.UpArrow:
                    Option = (Option - 1 + MenuItems.Count) % (MenuItems.Count);
                    break;

                case ConsoleKey.Enter:

                    foreach (var item in MenuItems)
                    {
                        if (item.Value.IsSelected)
                        {
                            userChoice = item.Value;
                        }
                    }
                    
                    if (userChoice?.MenuLabel is "Back" or "Return to main menu" )
                    {
                        closeMenu = Back();
                        if (userChoice?.MenuLabel is "Return to main menu")
                        {
                            closeMenu = Back(2);
                        }
                        
                        
                    } else if (userChoice?.MenuLabel == "Exit")
                    {
                        Environment.Exit(0);
                    } else if (PlayActions.Contains(userChoice?.MenuLabel))
                    {
                        closeMenu = Back();
                        Console.Clear();
                        
                    }

                    if (userChoice?.MethodToRun != null)
                    {
                        userChoice.MethodToRun!();
                    }
                    
                    
                    

                    break;
            }

            Console.WriteLine();
        } while (!closeMenu);
        

        return userChoice?.MenuLabel;
    }
}