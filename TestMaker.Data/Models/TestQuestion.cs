namespace TestMaker.Data.Models;

public abstract class TestQuestion: Question
{
    public List<TestAnswer> Answers { get; set; } =
    [
        new()
        {
            Answer = new Field
            {
                Value = string.Empty,
                Type = FieldType.Text
            },
            AnswerValue =  CorrectAnswer.A
        },
        new()
        {
            Answer = new Field
            {
                Value = string.Empty,
                Type = FieldType.Text
            },
            AnswerValue =  CorrectAnswer.B
        },
        new()
        {
            Answer = new Field
            {
                Value = string.Empty,
                Type = FieldType.Text
            },
            AnswerValue =  CorrectAnswer.C
        },
        new()
        {
            Answer = new Field
            {
                Value = string.Empty,
                Type = FieldType.Text
            },
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