namespace TestMaker.Data.Services.ServiceModels;

public class PageContent
{
    public PageContent(Languages language)
    {
        switch (language)
        {
            case Languages.English:
                AnotherSingleChoiceQuestion = "Another single-choice question";
                RandomSingleChoiceQuestion = "Random single-choice question";
                AnotherMultipleChoiceQuestion = "Another multiple choice question";
                RandomMultipleChoiceQuestion = "Random multiple choice question";
                AnotherOpenQuestion = "Another open question";
                RandomOpenQuestion = "Random open question";
                ShowAnswer = "Show answer";
                QuestionHeader = "Question: ";
                Answer = "Answer:";
                NotAllAnswers = "You don't select all correct answers.";
                TestMultiAllAnswers = "Correct!";
                CheckAnswers = "Check answers";
                break;
            case Languages.Polish:
                AnotherSingleChoiceQuestion = "Kolejne pytanie jednokrotnego wyboru";
                RandomSingleChoiceQuestion = "Losowe pytanie jednokrotnego wyboru";
                AnotherMultipleChoiceQuestion = "Kolejne pytanie wielokrotnego wyboru";
                RandomMultipleChoiceQuestion = "Losowe pytanie wielokrotnego wyboru";
                AnotherOpenQuestion = "Kolejne pytanie otwarte";
                RandomOpenQuestion = "Losowe pytanie otwarte";
                ShowAnswer = "Pokaż odpowiedź";
                QuestionHeader = "Pytanie: ";
                Answer = "Odpowiedź:";
                NotAllAnswers = "Nie zaznaczyłeś wszystkich poprawnych odpowiedzi.";
                TestMultiAllAnswers = "Brawo!";
                CheckAnswers = "Sprawdź odpowiedzi";
                break;
            case Languages.None:
                AnotherSingleChoiceQuestion = string.Empty;
                RandomSingleChoiceQuestion = string.Empty;
                AnotherMultipleChoiceQuestion = string.Empty;
                RandomMultipleChoiceQuestion = string.Empty;
                AnotherOpenQuestion = string.Empty;
                RandomOpenQuestion = string.Empty;
                ShowAnswer = string.Empty;
                QuestionHeader = string.Empty;
                NotAllAnswers = string.Empty;
                TestMultiAllAnswers = string.Empty;
                CheckAnswers = string.Empty;
                Answer = string.Empty;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(language), language, null);
        }
    }

    public string AnotherSingleChoiceQuestion { get; set; }
    public string RandomSingleChoiceQuestion { get; set; }
    public string AnotherMultipleChoiceQuestion { get; set; }
    public string RandomMultipleChoiceQuestion { get; set; }
    public string AnotherOpenQuestion { get; set; }
    public string RandomOpenQuestion { get; set; }

    public string ShowAnswer { get; set; }
    public string Answer {get; set;}
    public string QuestionHeader { get; set; }
    public string NotAllAnswers { get; set; }
    public string TestMultiAllAnswers { get; set; }
    public string CheckAnswers { get; set; }

    public bool IsEmpty => string.IsNullOrEmpty(AnotherSingleChoiceQuestion)
                           || string.IsNullOrEmpty(RandomSingleChoiceQuestion)
                           || string.IsNullOrEmpty(AnotherMultipleChoiceQuestion)
                           || string.IsNullOrEmpty(RandomMultipleChoiceQuestion)
                           || string.IsNullOrEmpty(AnotherOpenQuestion)
                           || string.IsNullOrEmpty(RandomOpenQuestion)
                           || string.IsNullOrEmpty(ShowAnswer)
                           || string.IsNullOrEmpty(Answer)
                           || string.IsNullOrEmpty(QuestionHeader)
                           || string.IsNullOrEmpty(NotAllAnswers)
                           || string.IsNullOrEmpty(TestMultiAllAnswers)
                           || string.IsNullOrEmpty(CheckAnswers);
}

public enum Languages
{
    None,
    English,
    Polish
}