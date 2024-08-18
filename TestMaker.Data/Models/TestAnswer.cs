namespace TestMaker.Data.Models;

public class TestAnswer : ICloneable
{
    public string Answer { get; set; } = string.Empty;
    public CorrectAnswer AnswerValue { get; set; }
    public object Clone()
    {
        return new TestAnswer
        {
            Answer = Answer,
            AnswerValue = AnswerValue,
        };
    }
}
