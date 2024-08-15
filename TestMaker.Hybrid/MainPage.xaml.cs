using System.Text;
using System.Text.Json;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.AspNetCore.Components.WebView;
using TestMaker.Data.Models;
using TestMaker.Hybrid.Messages;

namespace TestMaker.Hybrid;

public partial class MainPage : ContentPage
{
    private readonly IFileSaver _fileSaver;
    public MainPage(IFileSaver saver)
    {
        InitializeComponent();
        _fileSaver = saver;
        WeakReferenceMessenger.Default.Register<Project>(this, async (r, message) =>
        {
            await SaveProjectToFile(message);
        });
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

    private void SaveFile(object sender, EventArgs e)
    {
        WeakReferenceMessenger.Default.Send(new SaveFileClickedMessage());
    }

    private async Task SaveProjectToFile(Project project)
    {
        using var stream = new MemoryStream(Encoding.Default.GetBytes($"Saved project: {project.Name}"));
        var fileSaverResult = await _fileSaver.SaveAsync("test.txt", stream);
        fileSaverResult.EnsureSuccess();
        await Toast.Make($"File is saved: {fileSaverResult.FilePath}").Show();
    }
}