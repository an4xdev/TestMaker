namespace TestMaker.Data.Models;

public class Project
{
    public string Name { get; set; } = string.Empty;

    public List<Question> Questions { get; set; } = [];
}
