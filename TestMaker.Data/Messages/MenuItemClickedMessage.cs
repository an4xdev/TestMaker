namespace TestMaker.Data.Messages;

public class MenuItemClickedMessage(string selectedOption)
{
    public string SelectedOption { get; } = selectedOption;
}
