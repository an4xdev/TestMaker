namespace TestMaker.Hybrid.Messages;

public class MenuItemClickedMessage(string selectedOption)
{
    public string SelectedOption { get; } = selectedOption;
}
