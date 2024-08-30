namespace TestMaker.Data.Messages;

public enum Theme
{
    Light,
    Dark,
    System
}

public class ThemeChangedMessage
{
    public Theme Theme { get; set; }
}
