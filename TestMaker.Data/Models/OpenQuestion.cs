namespace TestMaker.Data.Models;

public class OpenQuestion : Question
{
    public string Answer { get; set; } = string.Empty;
    public override object Clone()
    {
        return new OpenQuestion
        {
            ID = ID,
            QuestionText = QuestionText,
            Answer = Answer,
        };
    }
}
