namespace TestMaker.Data.Models;

public abstract class TestQuestion: Question
{
    public List<TestAnswer> Answers { get; set; } =
    [
        new()
        {
            Answer = string.Empty,
            AnswerValue =  CorrectAnswer.A
        },
        new()
        {
            Answer = string.Empty,
            AnswerValue =  CorrectAnswer.B
        },
        new()
        {
            Answer = string.Empty,
            AnswerValue =  CorrectAnswer.C
        },
        new()
        {
            Answer = string.Empty,
            AnswerValue =  CorrectAnswer.D
        }
    ];

    public CorrectAnswer GetMaxCorrectAnswer()
    {
        var temp = Answers.Select(a => (int)a.AnswerValue).Max();
        if ((CorrectAnswer)temp == CorrectAnswer.Z)
        {
            return CorrectAnswer.Incorrect;
        }
        return (CorrectAnswer)temp + 1;
    }
}