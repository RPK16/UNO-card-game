namespace MenuSystem;

public class Menu
{
    public string? Title { get; set; }
    //public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    public Dictionary<string, MenuItem> MenuItems { get; set; } = new Dictionary<string, MenuItem>();
    public int Option = 0;

    private const string MenuSeparator = "======================";
    private static readonly string[] ReservedShortcuts = new string[] { "x", "b" };

    public Menu(string? title, List<MenuItem> menuItems)
    {
        Title = title;
        foreach (var menuItem in menuItems)
        {
            if (ReservedShortcuts.Contains(menuItem.Shortcut.ToLower()))
            {
                throw new ApplicationException($"This shortcut '{menuItem.Shortcut.ToLower()}' is not allowed");
            }

            if (MenuItems.ContainsKey(menuItem.Shortcut.ToLower()))
            {
                throw new ApplicationException($"This shortcut '{menuItem.Shortcut.ToLower()}' is already in the menu");
            }
            MenuItems[menuItem.Shortcut.ToLower()] = menuItem;
        }
    }

    public void ItemWrite(MenuItem item)
    {
        var color = "";
        if (item.IsSelected)
        { 
            color = " \u001b[32m";
        }
        Console.WriteLine(color + item.Shortcut + ") " + item.MenuLabel + "\u001b[0m");
    }

    private void Draw()
    {
        if (!string.IsNullOrWhiteSpace(Title))
        {
            Console.WriteLine(Title);
            Console.WriteLine(MenuSeparator);
        } 

        foreach (var menuItem in MenuItems)
        {
            menuItem.Value.IsSelected = MenuItems.ElementAt(Option).Key == menuItem.Key;
            ItemWrite(menuItem.Value);
            //Console.WriteLine(menuItem.Value.Shortcut);
        }
        
        Console.WriteLine("b) Back");
        Console.WriteLine("x) Exit");
        
        Console.WriteLine(MenuSeparator);
        Console.Write($"Your Choice:");
    }

    public string? Run()
    {
        Console.Clear();
        ConsoleKeyInfo key;
        
        var userChoice = " ";
        do
        {
            Console.Clear();
            Draw();

            key = Console.ReadKey();

            switch (key.Key)
            {
                case ConsoleKey.DownArrow:
                    Option = (Option + 1) % MenuItems.Count;
                    break;
                
                case ConsoleKey.UpArrow:
                    Option = (Option - 1 + MenuItems.Count) % MenuItems.Count;
                    break;
                
                case ConsoleKey.Enter:
                    userChoice = Console.ReadLine()?.Trim();
                    var selectedShortcut = MenuItems.ElementAt(Option).Key;
                    if (MenuItems.ContainsKey(selectedShortcut))
                    {
                        if (MenuItems[selectedShortcut].MethodToRun != null)
                        {
                            var result = MenuItems[selectedShortcut].MethodToRun!();
                            if (result?.ToLower() == "x")
                            {
                                userChoice = "x";
                            }
                        }
                    }
                    break;
            }
            
            Console.WriteLine();
            
        } while (!ReservedShortcuts.Contains(userChoice));

        return userChoice;

    }
   }