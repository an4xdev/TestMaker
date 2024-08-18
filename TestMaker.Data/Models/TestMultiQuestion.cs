namespace TestMaker.Data.Models;

public class TestMultiQuestion : Question
{
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
}
