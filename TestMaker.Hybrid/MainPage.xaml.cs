using System.Text;
using System.Text.Json;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.Messaging;
using TestMaker.Data.Messages;
using TestMaker.Data.Models;
using TestMaker.Data.Services;

namespace TestMaker.Hybrid;

public partial class MainPage
{
    private readonly IFileSaver _fileSaver;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly IMessenger _messenger;
    private readonly PickOptions _pickOptionsImage = PickOptions.Images;
    private readonly PickOptions _pickOptionsMarkdown;
    private readonly PickOptions _pickOptionsProject;
    private readonly IShowNotification _showNotification;
    private readonly string[] _supportedPhotoExtensions;

    public MainPage(IFileSaver saver, IMessenger messenger, IShowNotification showNotification)
    {
        InitializeComponent();
        _fileSaver = saver;
        _messenger = messenger;
        _showNotification = showNotification;
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        // project
        var filePickerFileTypeProject = new FilePickerFileType(
            new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, [".tmps"] }, // file extension
            });
        _pickOptionsProject = new PickOptions
        {
            PickerTitle = "Please select project file.",
            FileTypes = filePickerFileTypeProject,
        };

        // markdown
        var filePickerFileTypeMarkdown = new FilePickerFileType(
            new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, [".md"] }, // file extension
            });
        _pickOptionsMarkdown = new PickOptions
        {
            PickerTitle = "Please select markdown file.",
            FileTypes = filePickerFileTypeMarkdown,
        };

        // response from blazor on save project menu 
        _messenger.Register<SaveFileClickedMessageResponse>(this,
            async (_, message) => { await SaveProjectToFile(message.Project); });

        _messenger.Register<GeneratePageClickedMessageResponse>(this,
            async (_, message) => { await GeneratePage(message); });

        _messenger.Register<LoadPhotoToAnswerRequest>(this,
            async (_, message) => { await LoadPhotoFromMarkdown(message); });

        _messenger.Register<SaveFileWhenClosingResponse>(this, async (_, message) =>
        {
            if (message.Project != null)
            {
                await SaveProjectToFile(message.Project);
            }

            Application.Current?.CloseWindow(Application.Current.MainPage?.Window!);
        });

        _supportedPhotoExtensions = ["jpg", "jpeg", "png", "gif", "bmp", "webp"];
    }

    private void OnThemeItemClicked(object sender, EventArgs e)
    {
        if (sender is not MenuFlyoutItem menuItem) return;
        var selectedOption = menuItem.Text;
        if (Enum.TryParse(selectedOption, out Theme theme))
        {
            _messenger.Send(new ThemeChangedMessage { Theme = theme });
        }
    }

    private void SaveFileItemClicked(object sender, EventArgs e)
    {
        _messenger.Send(new SaveFileClickedMessage());
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
        catch (Exception)
        {
            // await Toast.Make(e.Message).Show();
            await Toast.Make("An error occured with saving file").Show();
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
            var result = await FilePicker.Default.PickAsync(_pickOptionsProject);
            if (result == null) return;
            if (!result.FileName.EndsWith(".tmps", StringComparison.OrdinalIgnoreCase)) return;
            await using var stream = await result.OpenReadAsync();
            var project = JsonSerializer.Deserialize<Project>(stream, _jsonSerializerOptions) ??
                          throw new InvalidOperationException();
            await Toast.Make($"Opened: {project.Name}").Show();
            _messenger.Send(new LoadProjectFromFileMessage
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
        _messenger.Send(new NewProjectClickedMessage());
    }

    private async void LoadProjectFromMarkdownClicked(object sender, EventArgs e)
    {
        await LoadProjectFromMarkdown();
    }

    private async Task LoadProjectFromMarkdown()
    {
        var result = await FilePicker.Default.PickAsync(_pickOptionsMarkdown);
        if (result == null) return;
        if (!result.FileName.EndsWith(".md", StringComparison.OrdinalIgnoreCase)) return;
        await using var stream = await result.OpenReadAsync();
        var reader = new StreamReader(stream);

        if (reader.EndOfStream)
        {
            await Toast.Make("Empty markdown file").Show();
            return;
        }

        var wholeFile = await reader.ReadToEndAsync();

        var project =
            await QuestionParser.ParseProjectFromMarkdown(_showNotification, wholeFile.Split(Environment.NewLine),
                result.FileName.Split('.')[0]);

        if (project == null) return;

        _messenger.Send(new LoadProjectFromFileMessage
        {
            Project = project
        });
    }

    private async Task LoadPhotoFromMarkdown(LoadPhotoToAnswerRequest request)
    {
        var result = await FilePicker.Default.PickAsync(_pickOptionsImage);

        if (result == null) return;
        await using var stream = await result.OpenReadAsync();

        if (stream.Length == 0)
        {
            await Toast.Make("Selected image is empty").Show();
            return;
        }

        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        var imageBytes = memoryStream.ToArray();

        var extension = Path.GetExtension(result.FileName).TrimStart('.').ToLower();

        if (!_supportedPhotoExtensions.Contains(extension))
        {
            await Toast.Make($"This image type: {extension} isn't supported").Show();
            return;
        }

        var data = $"data:image/{extension};base64,{Convert.ToBase64String(imageBytes)}";

        _messenger.Send(new LoadPhotoToAnswerResponse
        {
            QuestionId = request.QuestionId,
            PhotoBase64Data = data
        });
    }

    private void GeneratePageClicked(object sender, EventArgs e)
    {
        _messenger.Send(new GeneratePageClickedMessage());
    }

    private async Task GeneratePage(GeneratePageClickedMessageResponse response)
    {
        var htmlBuilderService = new HtmlBuilderService();
        htmlBuilderService = htmlBuilderService
            .AddHead(response.Language, response.ProjectName)
            .AddBody(response.ProjectName, response.PageContent)
            .AddScript(response.PageContent);
        if (response.Questions.TestOneQuestionsExists())
        {
            htmlBuilderService = htmlBuilderService.AddTestOneQuestionsScripts();
        }

        if (response.Questions.TestMultiQuestionsExists())
        {
            htmlBuilderService = htmlBuilderService.AddTestMultiQuestionsScripts();
        }

        if (response.Questions.OpenQuestionsExists())
        {
            htmlBuilderService = htmlBuilderService.AddOpenQuestionsScript();
        }

        htmlBuilderService = htmlBuilderService.AddQuestions(response.Questions);
        try
        {
            var jsonString = htmlBuilderService.Collect();
            using var stream = new MemoryStream(Encoding.Default.GetBytes(jsonString));
            var fileSaverResult = await _fileSaver.SaveAsync($"{response.ProjectName}.html", stream);
            fileSaverResult.EnsureSuccess();
            await Toast.Make($"File is saved: {fileSaverResult.FilePath}").Show();
        }
        catch (Exception e)
        {
            await Toast.Make(e.Message).Show();
        }
    }

    private void CloseApplication(object sender, EventArgs e)
    {
        _messenger.Send(new SaveFileWhenClosing());
    }

    private void OpenInfo(object sender, EventArgs e)
    {
        _messenger.Send(new OpenInfo());
    }
}