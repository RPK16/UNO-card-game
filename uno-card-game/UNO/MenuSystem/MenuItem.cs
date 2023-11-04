namespace MenuSystem;

public class MenuItem
{
    public string MenuLabel { get; set; } = default!;
    public string Shortcut { get; set; } = default!;

    public bool IsSelected { get; set; } = false;
    

    public Func<string?>? MethodToRun { get; set; } = null;
}
