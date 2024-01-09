namespace Domain;

public class GameOptions
{
    public int GameSpeed { get; set; } = 1000;
    public int AiSpeed { get; set; } = 3000;
    public bool AutoSave { get; set; } = false;

    public override string ToString() => $"GameSpeed: {GameSpeed}, Autosave: {(AutoSave ? "on" : "off")}";

    public string Autosaveonoff()
    {
        return AutoSave ? "On" : "Off";
    }
    
}