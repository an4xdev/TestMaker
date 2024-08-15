namespace TestMaker.Data.Models;

public class TestOneQuestion : Question
{
    public List<TestAnswer> Answers { get; set; } = [];
    public CorrectAnswer CorrectAnswer { get; set; }
}


