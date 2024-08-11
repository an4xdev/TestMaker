namespace TestMaker.Data;

public class Project
{
    public string Name { get; set; } = string.Empty;

    public List<Question> Questions { get; set; } = [];
}
