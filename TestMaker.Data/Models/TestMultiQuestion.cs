namespace TestMaker.Data.Models;

public class TestMultiQuestion : Question
{
    public TestMultiQuestion()
    {
        Answers =
        [
            new TestAnswer
            {
                Answer = string.Empty,
                AnswerValue = CorrectAnswer.A
            },
            new TestAnswer
            {
                Answer = string.Empty,
                AnswerValue = CorrectAnswer.B
            },
            new TestAnswer
            {
                Answer = string.Empty,
                AnswerValue = CorrectAnswer.C
            },
            new TestAnswer
            {
                Answer = string.Empty,
                AnswerValue = CorrectAnswer.D
            }
        ];
    }
    public List<TestAnswer> Answers { get; set; } = [];
    public List<CorrectAnswer> CorrectAnswers { get; set; } = [];
    public override object Clone()
    {
        return new TestMultiQuestion
        {
            ID = ID,
            QuestionText = QuestionText,
            Answers = Answers.Select(answer => (TestAnswer)answer.Clone()).ToList(),
            CorrectAnswers = CorrectAnswers.Select(correct => correct).ToList(),
        };
    }
    
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
