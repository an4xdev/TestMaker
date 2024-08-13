namespace TestMaker.Data;

public class TestOneQuestion : Question
{
    public List<TestAnswer> Answers { get; set; } = [];
    public CorrentAnswer CorrentAnswer { get; set; }
}


