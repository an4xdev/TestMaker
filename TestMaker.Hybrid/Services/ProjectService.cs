using TestMaker.Data.Models;
using TestMaker.Data.Services;
using TestMaker.Data.Services.ServiceModels;

namespace TestMaker.Hybrid.Services;

public class ProjectService : IProjectService
{
    public void AddQuestion(Project project, Question question)
    {
        project.Questions.Add(question);
    }

    public ServiceResponse CreateNew()
    {
        throw new NotImplementedException();
    }

    public ServiceResponse DeleteQuestion(Project project, Guid ID)
    {
        ServiceResponse result = new()
        {
            IsSuccess = project.Questions.RemoveAll(x => x.ID == ID) == 1,
        };

        if (!result.IsSuccess)
        {
            result.Message = "Unknown question to delete.";
        }
        return result;
    }

    public ServiceResponse EditQuestion(Project project, Guid originalID, Question edited)
    {
        ServiceResponse taskDone = new();

        var questionInList = project.Questions.Find(q => q.ID == originalID);

        if (questionInList == null)
        {
            taskDone.IsSuccess = false;
            taskDone.Message = "Uknown question to edit.";
            return taskDone;
        }

        questionInList = edited;
        taskDone.IsSuccess = true;
        return taskDone;
    }

    public Question? GetQuestionByID(Project project, Guid ID)
    {
        return project.Questions.Find(q => q.ID == ID);
    }

    public List<Question> GetQuestions(Project project)
    {
        return project.Questions;
    }

    public Question? GetRandomQuestion(Project project, QuestionType type)
    {
        List<Question> temp = [];

        foreach (var question in project.Questions)
        {
            if (type == QuestionType.TestOne && question is TestOneQuestion)
            {
                temp.Add(question);
            }
            else if (type == QuestionType.TestMulti && question is TestMultiQuestion)
            {
                temp.Add(question);
            }
            else if (type == QuestionType.Open && question is OpenQuestion)
            {
                temp.Add(question);
            }
        }

        Random random = new();

        if (temp.Count == 0)
        {
            return null;
        }

        return temp[random.Next(temp.Count)];
    }

    private readonly string[] Lorem = { "Lorem", "ipsum", "dolor", "sit", "amet,", "consectetur", "adipiscing", "elit.", "Integer", "nibh", "lectus,", "placerat", "placerat", "erat", "sed,", "semper", "laoreet", "urna.", "Fusce", "vestibulum,", "ex", "mollis", "vulputate", "commodo,", "massa", "erat", "rutrum", "nisl,", "finibus", "vehicula", "augue", "nulla", "ac", "ipsum.", "Sed", "aliquet", "maximus", "ex", "porta", "condimentum.", "Duis", "pulvinar", "imperdiet", "pellentesque.", "Nunc", "gravida", "euismod", "tincidunt.", "Mauris", "ac", "neque", "nunc.", "Curabitur", "ornare", "quam", "et", "quam", "posuere", "euismod", "ac", "in", "neque.", "Vestibulum", "eleifend", "semper", "pretium.", "Donec", "mi", "est,", "aliquet", "quis", "finibus", "vitae,", "tincidunt", "vitae", "ligula.", "Nulla", "quis", "sodales", "augue,", "a", "placerat", "erat.", "Aliquam", "lacinia,", "leo", "in", "accumsan", "lacinia,", "turpis", "mi", "bibendum", "metus,", "non", "tincidunt", "ante", "augue", "in", "nisi.", "Quisque", "placerat", "turpis", "ut", "mauris", "varius", "ultrices.", "Vestibulum", "sit", "amet", "rutrum", "metus,", "id", "efficitur", "dui.", "Mauris", "commodo", "magna", "nulla,", "rhoncus", "aliquet", "metus", "faucibus", "at.", "Cras", "posuere,", "nunc", "a", "facilisis", "suscipit,", "ipsum", "tortor", "fermentum", "erat,", "sit", "amet", "consectetur", "lorem", "nulla", "sit", "amet", "elit.", "Morbi", "et", "aliquet", "velit." };

    private string GetRandomString(int factor)
    {
        Random random = new();
        List<string> list = [];

        int count = random.Next(Lorem.Length / factor, Lorem.Length / factor * 3);

        for (int i = 0; i < count; i++)
        {
            list.Add(Lorem[random.Next(Lorem.Length)]);
        }

        return string.Join(" ", list);
    }
    public void MockData(Project project)
    {
        const int questionFactor = 25;
        const int answerFactor = 8;

        project.Name = "TEST";

        Random random = new();

        const int N = 25;

        for (int i = 0; i < N; i++)
        {
            if (i < N / 3)
            {
                project.Questions.Add(new TestOneQuestion
                {
                    ID = Guid.NewGuid(),
                    QuestionText = GetRandomString(questionFactor),
                    // Answers = [new TestAnswer { Answer = "Duis in dictum leo.", AnswerValue = 0 }, new TestAnswer { Answer = "Morbi viverra, enim in porta tincidunt, metus ipsum imperdiet velit, in facilisis enim odio vitae leo.", AnswerValue = 1 }, new TestAnswer { Answer = "Vestibulum sit amet pulvinar velit, ut ultricies eros", AnswerValue = 2}, new TestAnswer { Answer = "Fusce vel velit commodo, maximus eros quis, faucibus nulla.", AnswerValue = 3 }],
                    Answers = [new TestAnswer { Answer = GetRandomString(answerFactor), AnswerValue = CorrectAnswer.A }, new TestAnswer { Answer = GetRandomString(answerFactor), AnswerValue = CorrectAnswer.B }, new TestAnswer { Answer = GetRandomString(answerFactor), AnswerValue = CorrectAnswer.C }, new TestAnswer { Answer = GetRandomString(answerFactor), AnswerValue = CorrectAnswer.D }],
                    CorrectAnswer = (CorrectAnswer)random.Next(4),
                });
            }
            else if (i < (N / 3) * 2)
            {

                int rand = random.Next(4);

                if (rand == 0)
                {
                    rand++;
                }

                List<CorrectAnswer> answers = new(rand);

                for (int j = 0; j < rand; j++)
                {
                    answers.Add((CorrectAnswer)j);
                }

                project.Questions.Add(new TestMultiQuestion
                {
                    ID = Guid.NewGuid(),
                    QuestionText = GetRandomString(questionFactor),
                    // Answers = [new TestAnswer { Answer = "Duis in dictum leo.", AnswerValue = 0 }, new TestAnswer { Answer = "Morbi viverra, enim in porta tincidunt, metus ipsum imperdiet velit, in facilisis enim odio vitae leo.", AnswerValue = 1 }, new TestAnswer { Answer = "Vestibulum sit amet pulvinar velit, ut ultricies eros", AnswerValue = 2 }, new TestAnswer { Answer = "Fusce vel velit commodo, maximus eros quis, faucibus nulla.", AnswerValue = 3 }],
                    Answers = [new TestAnswer { Answer = GetRandomString(answerFactor), AnswerValue = CorrectAnswer.A }, new TestAnswer { Answer = GetRandomString(answerFactor), AnswerValue = CorrectAnswer.B }, new TestAnswer { Answer = GetRandomString(answerFactor), AnswerValue = CorrectAnswer.C }, new TestAnswer { Answer = GetRandomString(answerFactor), AnswerValue = CorrectAnswer.D }],
                    CorrectAnswers = answers
                });
            }
            else
            {
                project.Questions.Add(new OpenQuestion
                {
                    ID = Guid.NewGuid(),
                    QuestionText = GetRandomString(questionFactor),
                    Answer = string.Join(" ", Lorem)
                });
            }
        }
    }

    public ServiceResponse SaveProject(Project project)
    {
        throw new NotImplementedException();
    }
}
