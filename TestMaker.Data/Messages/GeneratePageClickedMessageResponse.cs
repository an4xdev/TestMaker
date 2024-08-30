using TestMaker.Data.Models;
using TestMaker.Data.Services.ServiceModels;

namespace TestMaker.Data.Messages;

public class GeneratePageClickedMessageResponse
{
    public string Language { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public PageContent PageContent { get; set; } = new PageContent(Languages.English);
    public string ShowOpenQuestionText { get; set; } = string.Empty;
    public List<Question> Questions { get; set; } = [];
}