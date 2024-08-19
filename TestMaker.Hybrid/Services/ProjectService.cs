using CommunityToolkit.Mvvm.Messaging;
using TestMaker.Data.Models;
using TestMaker.Data.Services;
using TestMaker.Data.Services.ServiceModels;
using TestMaker.Hybrid.Messages;

namespace TestMaker.Hybrid.Services;

public class ProjectService : IProjectService
{
    public void AddQuestion(Project project, Question question)
    {
        project.Questions.Add(question);
    }
    public ServiceResponse DeleteQuestion(Project project, Guid id)
    {
        ServiceResponse response = new()
        {
            IsSuccess = project.Questions.RemoveAll(x => x.ID == id) == 1,
        };

        if (!response.IsSuccess)
        {
            response.Message = "Unknown question to delete.";
        }

        response.Message = "Question successfully updated.";
        return response;
    }

    public ServiceResponse EditQuestion(Project project, Guid originalId, Question edited)
    {
        ServiceResponse response = new();

        var index = project.Questions.FindIndex(q => q.ID == originalId);

        if (index == -1)
        {
            response.IsSuccess = false;
            response.Message = "Unknown question to edit.";
            return response;
        }

        project.Questions[index] = edited;

        response.Message = "Question successfully updated.";
        response.IsSuccess = true;
        return response;
    }

    public Question? GetQuestionById(Project project, Guid id)
    {
        return project.Questions.Find(q => q.ID == id);
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
            switch (type)
            {
                case QuestionType.TestOne when question is TestOneQuestion:
                case QuestionType.TestMulti when question is TestMultiQuestion:
                case QuestionType.Open when question is OpenQuestion:
                    temp.Add(question);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        Random random = new();

        return temp.Count == 0 ? null : temp[random.Next(temp.Count)];
    }

    private readonly string[] _lorem = ["Lorem", "ipsum", "dolor", "sit", "amet,", "consectetur", "adipiscing", "elit.", "Integer", "nibh", "lectus,", "placerat", "placerat", "erat", "sed,", "semper", "laoreet", "urna.", "Fusce", "vestibulum,", "ex", "mollis", "vulputate", "commodo,", "massa", "erat", "rutrum", "nisl,", "finibus", "vehicula", "augue", "nulla", "ac", "ipsum.", "Sed", "aliquet", "maximus", "ex", "porta", "condimentum.", "Duis", "pulvinar", "imperdiet", "pellentesque.", "Nunc", "gravida", "euismod", "tincidunt.", "Mauris", "ac", "neque", "nunc.", "Curabitur", "ornare", "quam", "et", "quam", "posuere", "euismod", "ac", "in", "neque.", "Vestibulum", "eleifend", "semper", "pretium.", "Donec", "mi", "est,", "aliquet", "quis", "finibus", "vitae,", "tincidunt", "vitae", "ligula.", "Nulla", "quis", "sodales", "augue,", "a", "placerat", "erat.", "Aliquam", "lacinia,", "leo", "in", "accumsan", "lacinia,", "turpis", "mi", "bibendum", "metus,", "non", "tincidunt", "ante", "augue", "in", "nisi.", "Quisque", "placerat", "turpis", "ut", "mauris", "varius", "ultrices.", "Vestibulum", "sit", "amet", "rutrum", "metus,", "id", "efficitur", "dui.", "Mauris", "commodo", "magna", "nulla,", "rhoncus", "aliquet", "metus", "faucibus", "at.", "Cras", "posuere,", "nunc", "a", "facilisis", "suscipit,", "ipsum", "tortor", "fermentum", "erat,", "sit", "amet", "consectetur", "lorem", "nulla", "sit", "amet", "elit.", "Morbi", "et", "aliquet", "velit."
    ];

    private string GetRandomString(int factor)
    {
        Random random = new();
        List<string> list = [];

        var count = random.Next(_lorem.Length / factor, _lorem.Length / factor * 3);

        for (var i = 0; i < count; i++)
        {
            list.Add(_lorem[random.Next(_lorem.Length)]);
        }

        return string.Join(" ", list);
    }
    public void MockData(Project project)
    {
        const int questionFactor = 25;
        const int answerFactor = 8;

        project.Name = "TEST";

        Random random = new();

        const int n = 25;

        for (var i = 0; i < n; i++)
        {
            switch (i)
            {
                case < n / 3:
                    project.Questions.Add(new TestOneQuestion
                    {
                        ID = Guid.NewGuid(),
                        QuestionText = GetRandomString(questionFactor),
                        // Answers = [new TestAnswer { Answer = "Duis in dictum leo.", AnswerValue = 0 }, new TestAnswer { Answer = "Morbi viverra, enim in porta tincidunt, metus ipsum imperdiet velit, in facilisis enim odio vitae leo.", AnswerValue = 1 }, new TestAnswer { Answer = "Vestibulum sit amet pulvinar velit, ut ultricies eros", AnswerValue = 2}, new TestAnswer { Answer = "Fusce vel velit commodo, maximus eros quis, faucibus nulla.", AnswerValue = 3 }],
                        Answers = [new TestAnswer { Answer = GetRandomString(answerFactor), AnswerValue = CorrectAnswer.A }, new TestAnswer { Answer = GetRandomString(answerFactor), AnswerValue = CorrectAnswer.B }, new TestAnswer { Answer = GetRandomString(answerFactor), AnswerValue = CorrectAnswer.C }, new TestAnswer { Answer = GetRandomString(answerFactor), AnswerValue = CorrectAnswer.D }],
                        CorrectAnswer = (CorrectAnswer)random.Next(4),
                    });
                    break;
                case < (n / 3) * 2:
                {
                    var rand = random.Next(4);

                    if (rand == 0)
                    {
                        rand++;
                    }

                    List<CorrectAnswer> answers = new(rand);

                    for (var j = 0; j < rand; j++)
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
                    break;
                }
                default:
                    project.Questions.Add(new OpenQuestion
                    {
                        ID = Guid.NewGuid(),
                        QuestionText = GetRandomString(questionFactor),
                        Answer = string.Join(" ", _lorem)
                    });
                    break;
            }
        }
    }

    public void SaveProject(Project project)
    {
        WeakReferenceMessenger.Default.Send(new SaveFileClickedMessageResponse()
        {
            Project = project
        });
    }

    public bool QuestionExists(Project project, Guid id)
    {
        return project.Questions.Any(q => q.ID == id);
    }
}
