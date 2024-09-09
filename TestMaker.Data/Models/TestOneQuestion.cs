namespace TestMaker.Data.Models;

public class TestOneQuestion : TestQuestion
{
    public CorrectAnswer CorrectAnswer { get; set; }
    public override object Clone()
    {
        return new TestOneQuestion
        {
            ID = ID,
            QuestionText = QuestionText,
            Answers = Answers.Select(answer => (TestAnswer)answer.Clone()).ToList(),
            CorrectAnswer = CorrectAnswer,
        };
    }
    
}


