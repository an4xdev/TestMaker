using System.Text;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.Messaging;
using TestMaker.Hybrid.Messages;

namespace TestMaker.Hybrid;

public partial class MainPage : ContentPage
{
    private readonly IFileSaver _fileSaver;
    public MainPage(IFileSaver saver)
    {
        InitializeComponent();
        _fileSaver = saver;
    }

    private void OnMenuItemClicked(object sender, EventArgs e)
    {
        if (sender is not MenuFlyoutItem menuItem) return;
        var selectedOption = menuItem.Text;
        WeakReferenceMessenger.Default.Send(new MenuItemClickedMessage(selectedOption));
    }

    private void OnThemeItemClicked(object sender, EventArgs e)
    {
        if (sender is not MenuFlyoutItem menuItem) return;
        var selectedOption = menuItem.Text;
        if (Enum.TryParse(selectedOption, out Theme theme))
        {
            WeakReferenceMessenger.Default.Send(new ThemeChangedMessage { Theme = theme });
        }
    }

    public async void SaveFile(object sender, EventArgs e)
    {
        using var stream = new MemoryStream(Encoding.Default.GetBytes("Hello from the Community Toolkit!"));
        var fileSaverResult = await _fileSaver.SaveAsync("test.txt", stream);
        fileSaverResult.EnsureSuccess();
        await Toast.Make($"File is saved: {fileSaverResult.FilePath}").Show();
    }

}