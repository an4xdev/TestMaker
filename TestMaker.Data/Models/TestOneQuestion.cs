namespace TestMaker.Data.Models;

public class TestOneQuestion : Question
{
    public TestOneQuestion()
    {
        Answers = [
            new TestAnswer
            {
                Answer = string.Empty,
                AnswerValue =  CorrectAnswer.A
            },
            new TestAnswer
            {
                Answer = string.Empty,
                AnswerValue =  CorrectAnswer.B
            },
            new TestAnswer
            {
                Answer = string.Empty,
                AnswerValue =  CorrectAnswer.C
            },
            new TestAnswer
            {
                Answer = string.Empty,
                AnswerValue =  CorrectAnswer.D
            }
        ];
    }
    public List<TestAnswer> Answers { get; set; } = [];
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


