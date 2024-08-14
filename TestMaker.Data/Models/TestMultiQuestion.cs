namespace TestMaker.Data.Models;

public class TestMultiQuestion : Question
{
    public List<TestAnswer> Answers { get; set; } = [];
    public List<CorrentAnswer> CorrentAnswers { get; set; } = [];
}
