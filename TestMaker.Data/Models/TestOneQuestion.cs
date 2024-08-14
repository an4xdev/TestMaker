namespace TestMaker.Data.Models;

public class TestOneQuestion : Question
{
    public List<TestAnswer> Answers { get; set; } = [];
    public CorrentAnswer CorrentAnswer { get; set; }
}


