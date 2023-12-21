using System.Security.Cryptography;

namespace MenuSystem;

public class MenuItem
{
    public string MenuLabel { get; set; } = default!;

    public Func<string>? MenuLabelFunction { get; set; }
    //public int DisplayLevel { get; set; } = default!;
    

    public bool IsSelected { get; set; } = false;
    
    
    
    public string? ItemNr { get; set; } = null;
    public Func<string?>? MethodToRun { get; set; } = null;
    
    public Func<string?>? MenuToRun { get; set; } = null;
    
   // public Func<bool?>? CloseMenu { get; set; } = null;
}
