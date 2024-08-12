using CommunityToolkit.Mvvm.Messaging;
using TestMaker.Hybrid.Messages;

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

    private void OnThemeItemClicked(object sender, EventArgs e)
    {
        if (sender is MenuFlyoutItem menuItem)
        {
            string selectedOption = menuItem.Text;
            if (Theme.TryParse(selectedOption, out Theme theme))
            {
                WeakReferenceMessenger.Default.Send(new ThemeChangedMessage { Theme = theme });
            }
        }
    }

}