using TestMaker.Data.Models;

namespace TestMaker.Data.Services.ServiceModels;

public class QuestionParseResponse
{
    public Question? Question { get; set; } = null;
    public string? Message { get; set; } = null;
}