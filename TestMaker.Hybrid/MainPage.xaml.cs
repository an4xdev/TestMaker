﻿using System.Text;
using System.Text.Json;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.Messaging;
using TestMaker.Data.Messages;
using TestMaker.Data.Models;
using TestMaker.Data.Services;

namespace TestMaker.Hybrid;

public partial class MainPage : ContentPage
{
    private readonly IFileSaver _fileSaver;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly PickOptions _pickOptionsProject;
    private readonly PickOptions _pickOptionsMarkdown;
    private readonly IMessenger _messenger;
    public MainPage(IFileSaver saver, IMessenger messenger)
    {
        InitializeComponent();
        _fileSaver = saver;
        _messenger = messenger;
        _jsonSerializerOptions = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        
        // project
        var filePickerFileTypeProject = new FilePickerFileType(
            new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new[] { ".tmps" } }, // file extension
            });
        _pickOptionsProject =  new PickOptions
        {
            PickerTitle = "Please select project file.",
            FileTypes = filePickerFileTypeProject,
        };
        var filePickerFileTypeMarkdown = new FilePickerFileType(
            new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new[] { ".md" } }, // file extension
            });
        
        // markdown
        _pickOptionsMarkdown =  new PickOptions
        {
            PickerTitle = "Please select markdown file.",
            FileTypes = filePickerFileTypeMarkdown,
        };
        
        // response from blazor on save project menu 
        _messenger.Register<SaveFileClickedMessageResponse>(this, async (r, message) =>
        {
            await SaveProjectToFile(message.Project);
        });
        
        _messenger.Register<GeneratePageClickedMessageResponse>(this, async (r, message) =>
        {
            await GeneratePage(message);
        } );
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
            var result = await FilePicker.Default.PickAsync(_pickOptionsProject);
            if (result == null) return;
            if (!result.FileName.EndsWith(".tmps", StringComparison.OrdinalIgnoreCase)) return;
            await using var stream = await result.OpenReadAsync();
            var project = JsonSerializer.Deserialize<Project>(stream, _jsonSerializerOptions) ?? throw new InvalidOperationException();
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

    private async Task<Question?> ProcessQuestion(List<string> data)
    {
        var boldCounter = data.Count(s => s.Contains("**"));
        switch (boldCounter)
        {
            case 0 when data.Count == 2:
                return new OpenQuestion()
                {
                    ID = Guid.NewGuid(),
                    QuestionText = data[0].Split("##")[1],
                    Answer = data[1]
                };
            case 0 when data.Count != 2:
                await Toast.Make("According to the data read, this should be an open question. Unfortunately, an error was encountered.").Show();
                return null;
            case 1 when data.Count == 5:
            {
                var question = new TestOneQuestion
                {
                    ID = Guid.NewGuid(),
                    QuestionText = data[0].Split("##")[1]
                };
                for (var i = 1; i < data.Count; i++)
                {
                    var answer = new TestAnswer
                    {
                        Answer = data[i].Contains("**") ? data[i].Split("- **")[1].Split("**")[0] : data[i].Split("- ")[1],
                        AnswerValue = (CorrectAnswer)i - 1
                    };
                    if (data[i].Contains("**"))
                    {
                        question.CorrectAnswer = (CorrectAnswer)i - 1;
                    }
                    question.Answers.Add(answer);
                }
                return question;
            }
            case 1 when data.Count != 5:
                await Toast.Make("According to the data read, this should be an test question with one answer. Unfortunately, an error was encountered.").Show();
                return null;
            case > 1 and <= 4 when data.Count == 5:
            {
                var question = new TestMultiQuestion()
                {
                    ID = Guid.NewGuid(),
                    QuestionText = data[0].Split("##")[1],
                };

                for (var i = 1; i < data.Count; i++)
                {
                    var answer = new TestAnswer
                    {
                        Answer = data[i].Contains("**") ? data[i].Split("- **")[1].Split("**")[0] : data[i].Split("- ")[1],
                        AnswerValue = (CorrectAnswer)i - 1
                    };

                    if (data[i].Contains("**"))
                    {
                        question.CorrectAnswers.Add((CorrectAnswer)i - 1);
                    }
                    
                    question.Answers.Add(answer);
                }
                return question;
            }
            case > 1 and <= 4 when data.Count != 5:
                await Toast.Make("According to the data read, this should be an test question with multiple answers. Unfortunately, an error was encountered.").Show();
                return null;
        }
        
        return null;
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
        
        var project = new Project();

        var projectName = false;

        var data = new List<string>();
        
        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (line == null)
            {
                break;
            }
            
            if(line.Equals("")) continue;
            
            if (!projectName)
            {
                if (!line.StartsWith($"##"))
                {
                    project.Name = line.Split("#")[1];
                }
                else
                {
                    project.Name = result.FileName.Split('.')[0];
                    data.Add(line);
                }
                projectName = true;
                continue;
            }

            if (line.StartsWith("##") && data.Count == 2 || data.Count == 5)
            {
                var question = await ProcessQuestion(data);
                if (question != null)
                {
                    project.Questions.Add(question);
                    data.Clear();
                }
                else
                {
                    await Toast.Make("An error occurred while processing the question from the file.").Show();
                    return;
                }
            }

            data.Add(line);
        }

        if (data.Count is 2 or 5)
        {
            var question = await ProcessQuestion(data);
            if (question != null)
            {
                project.Questions.Add(question);
            }
            else
            {
                await Toast.Make("An error occurred while processing the question from the file.").Show();
                return;
            }
        }
        else
        {
            await Toast.Make("An error occurred while processing the question from the file.").Show();
            return;
        }
        
        _messenger.Send(new LoadProjectFromFileMessage
        {
            Project = project
        });
    }
    
    private void GeneratePageClicked(object sender, EventArgs e)
    {
        _messenger.Send(new GeneratePageClickedMessage());
    }

    private async Task GeneratePage(GeneratePageClickedMessageResponse response)
    {
        var htmlBuilderService = new HtmlBuilderService();
        htmlBuilderService = htmlBuilderService.AddHead(response.Language, response.ProjectName)
            .AddBody(response.ProjectName, response.PageContent)
            .AddScript(response.ShowOpenQuestionText)
            .AddQuestions(response.Questions);
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
        Application.Current?.CloseWindow(Application.Current.MainPage.Window);
    }
    
    private void OpenInfo(object sender, EventArgs e)
    {
        _messenger.Send(new OpenInfo());
    }
}