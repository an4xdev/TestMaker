using System.Text;
using System.Text.Json;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.Messaging;
using TestMaker.Data.Models;
using TestMaker.Hybrid.Messages;

namespace TestMaker.Hybrid;

public partial class MainPage : ContentPage
{
    private readonly IFileSaver _fileSaver;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly PickOptions _pickOptions;
    public MainPage(IFileSaver saver)
    {
        InitializeComponent();
        _fileSaver = saver;
        _jsonSerializerOptions = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var filePickerFileType = new FilePickerFileType(
            new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new[] { ".tmps" } }, // file extension
            });
        _pickOptions =  new PickOptions
        {
            PickerTitle = "Please select project file.",
            FileTypes = filePickerFileType,
        };
        WeakReferenceMessenger.Default.Register<SaveFileClickedMessageResponse>(this, async (r, message) =>
        {
            await SaveProjectToFile(message.Project);
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

    private void SaveFileItemClicked(object sender, EventArgs e)
    {
        WeakReferenceMessenger.Default.Send(new SaveFileClickedMessage());
    }

    private async Task SaveProjectToFile(Project project)
    {
        try
        {
            var jsonString = JsonSerializer.Serialize(project, _jsonSerializerOptions);
            using var stream = new MemoryStream(Encoding.Default.GetBytes(jsonString));
            var fileSaverResult = await _fileSaver.SaveAsync($"{project.Name}.tmps", stream);
            fileSaverResult.EnsureSuccess();
            await Toast.Make($"File is saved: {fileSaverResult.FilePath}").Show();
        }
        catch (Exception e)
        {
            await Toast.Make(e.Message).Show();
        }
       
    }

    private async void OpenFileItemClicked(object sender, EventArgs e)
    {
        await LoadFromFile();
    }

    private async Task LoadFromFile()
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(_pickOptions);
            if (result == null) return;
            if (!result.FileName.EndsWith(".tmps", StringComparison.OrdinalIgnoreCase)) return;
            await using var stream = await result.OpenReadAsync();
            var project = JsonSerializer.Deserialize<Project>(stream, _jsonSerializerOptions) ?? throw new InvalidOperationException();
            await Toast.Make($"Opened: {project.Name}").Show();
            WeakReferenceMessenger.Default.Send(new LoadProjectFromFileMessage
            {
                Project = project
            });
        }
        catch (Exception ex)
        {
            await Toast.Make(ex.Message).Show();
        }
    }

    private void NewProjectClicked(object sender, EventArgs e)
    {
        WeakReferenceMessenger.Default.Send(new NewProjectClickedMessage());
    }
}