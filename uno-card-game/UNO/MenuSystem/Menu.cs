namespace MenuSystem;

public class Menu
{
    public string? Title { get; set; }
    //public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    public Dictionary<string, MenuItem> MenuItems { get; set; } = new Dictionary<string, MenuItem>();

    private const string MenuSeparator = "======================";
    private static readonly string[] ForbiddenShortcuts = new string[] { "x", "b" };

    public Menu(string? title, List<MenuItem> menuItems)
    {
        Title = title;
        foreach (var menuItem in menuItems)
        {
            if (ForbiddenShortcuts.Contains(menuItem.Shortcut.ToLower()))
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

    private void Draw()
    {
        if (string.IsNullOrWhiteSpace(Title))
        {
            Console.WriteLine(Title);
            Console.WriteLine(MenuSeparator);
        }

        foreach (var menuItem in MenuItems)
        {
            Console.Write(menuItem.Key);
            Console.Write(") ");
            Console.WriteLine(menuItem.Value.Shortcut);
        }
        Console.WriteLine(MenuSeparator);
        Console.Write($"Your Choice:");
    }

    public string Run()
    {
        Draw();
        return "x";

    }
   }