using CommunityToolkit.Mvvm.Messaging;

namespace TestMaker.Hybrid;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

    }

    private void OnMenuItemClicked(object sender, EventArgs e)
    {
        if (sender is MenuFlyoutItem menuItem)
        {
            string selectedOption = menuItem.Text;
            WeakReferenceMessenger.Default.Send(new MenuItemClickedMessage(selectedOption));
        }
    }

}

public class MenuItemClickedMessage(string selectedOption)
{
    public string SelectedOption { get; } = selectedOption;
}
