using TestMaker.Data.Models;

namespace TestMaker.Data.Services;

public static class Utils
{
    public static bool TestOneQuestionsExists(this List<Question> questions)
    {
        return questions.OfType<TestOneQuestion>().Any();
    }

    public static bool TestMultiQuestionsExists(this List<Question> questions)
    {
        return questions.OfType<TestMultiQuestion>().Any();
    }

    public static bool OpenQuestionsExists(this List<Question> questions)
    {
        return questions.OfType<OpenQuestion>().Any();
    }
}