namespace TestMaker.Hybrid.Messages;

public enum Theme
{
    White,
    Dark,
    System
}

public class ThemeChangedMessage
{
    public Theme Theme { get; set; }
}
