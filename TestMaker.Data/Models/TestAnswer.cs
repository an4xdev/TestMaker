namespace TestMaker.Data.Models;

public class TestAnswer : ICloneable
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Field Answer { get; set; } = new();
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
