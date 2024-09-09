namespace TestMaker.Data.Models;

public class OpenQuestion : Question
{
    public Field Answer { get; set; } = new();
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
