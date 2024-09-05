namespace TestMaker.Data.Models;

public class TestAnswer : ICloneable
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Answer { get; set; } = string.Empty;
    public CorrectAnswer AnswerValue { get; set; }
    public object Clone()
    {
        return new TestAnswer
        {
            Id = Id,
            Answer = Answer,
            AnswerValue = AnswerValue,
        };
    }
}
