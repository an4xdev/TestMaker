namespace TestMaker.Data;

public class TestMultiQuestion : Question
{
    public string[] Questions { get; set; } = ["", "", "", ""];
    public List<CorrentAnswer> CorrentAnswers { get; set; } = [];
}
