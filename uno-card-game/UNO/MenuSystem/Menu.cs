namespace MenuSystem;

public class Menu
{
    public string? Title { get; set; }
    //public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    public Dictionary<int, MenuItem> MenuItems { get; set; } = new Dictionary<int, MenuItem>();
    public int Option = 0;
   // private int Level = 0;

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
            case EMenuLevel.First:
                MenuItems[counter] = new MenuItem(){MenuLabel = "Exit"};
                break;
            case EMenuLevel.Second:
                MenuItems[counter] = new MenuItem(){MenuLabel = "Back"};
                break;
            case EMenuLevel.Other:
                MenuItems[counter] = new MenuItem(){MenuLabel = "Back"};
                MenuItems[counter + 1] = new MenuItem(){MenuLabel = "Return to main menu"};
                break;
        }
    }

    public void ItemWrite(MenuItem item)
    {
        var color = "";
        if (item.IsSelected)
        { 
            color = " \u001b[32m";
        }
        Console.WriteLine(color + item.MenuLabel + "\u001b[0m");
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
            ItemWrite(menuItem.Value);
        }
    
    }

    public void Close()
    {
        
    }

    public string? Run()
    {
        var Exit = false;
        Console.Clear();
        ConsoleKeyInfo key;
        
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
                    if (MenuItems.ContainsKey(Option))
                        Console.Write(MenuItems);
                    {
                        if (MenuItems[Option].MethodToRun != null)
                        {
                            var result = MenuItems[Option].MethodToRun!();
                            if (MenuItems[Option].MenuLabel == "Exit")
                            {
                                Exit = true;
                            }
                        }
                    }
                    break;
            }
            
            Console.WriteLine();
            
        } while (!Exit);

        return "userChoice";

    }
   }