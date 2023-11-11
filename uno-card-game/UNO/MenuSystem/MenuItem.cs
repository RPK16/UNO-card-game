﻿namespace MenuSystem;

public class MenuItem
{
    public string MenuLabel { get; set; } = default!;
    //public int DisplayLevel { get; set; } = default!;

    public bool IsSelected { get; set; } = false;
    

    public Func<string?>? MethodToRun { get; set; } = null;
    
    public Func<string?>? MenuToRun { get; set; } = null;
    
   // public Func<bool?>? CloseMenu { get; set; } = null;
}
