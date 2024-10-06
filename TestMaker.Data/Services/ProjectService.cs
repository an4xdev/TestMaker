using CommunityToolkit.Mvvm.Messaging;
using TestMaker.Data.Messages;
using TestMaker.Data.Models;
using TestMaker.Data.Services;
using TestMaker.Data.Services.ServiceModels;

namespace TestMaker.Data.Services;

public class ProjectService(IMessenger messenger) : IProjectService
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

    private readonly string[] _lorem =
    [
        "Lorem", "ipsum", "dolor", "sit", "amet,", "consectetur", "adipiscing", "elit.", "Integer", "nibh", "lectus,",
        "placerat", "placerat", "erat", "sed,", "semper", "laoreet", "urna.", "Fusce", "vestibulum,", "ex", "mollis",
        "vulputate", "commodo,", "massa", "erat", "rutrum", "nisl,", "finibus", "vehicula", "augue", "nulla", "ac",
        "ipsum.", "Sed", "aliquet", "maximus", "ex", "porta", "condimentum.", "Duis", "pulvinar", "imperdiet",
        "pellentesque.", "Nunc", "gravida", "euismod", "tincidunt.", "Mauris", "ac", "neque", "nunc.", "Curabitur",
        "ornare", "quam", "et", "quam", "posuere", "euismod", "ac", "in", "neque.", "Vestibulum", "eleifend", "semper",
        "pretium.", "Donec", "mi", "est,", "aliquet", "quis", "finibus", "vitae,", "tincidunt", "vitae", "ligula.",
        "Nulla", "quis", "sodales", "augue,", "a", "placerat", "erat.", "Aliquam", "lacinia,", "leo", "in", "accumsan",
        "lacinia,", "turpis", "mi", "bibendum", "metus,", "non", "tincidunt", "ante", "augue", "in", "nisi.", "Quisque",
        "placerat", "turpis", "ut", "mauris", "varius", "ultrices.", "Vestibulum", "sit", "amet", "rutrum", "metus,",
        "id", "efficitur", "dui.", "Mauris", "commodo", "magna", "nulla,", "rhoncus", "aliquet", "metus", "faucibus",
        "at.", "Cras", "posuere,", "nunc", "a", "facilisis", "suscipit,", "ipsum", "tortor", "fermentum", "erat,",
        "sit", "amet", "consectetur", "lorem", "nulla", "sit", "amet", "elit.", "Morbi", "et", "aliquet", "velit."
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
                        Answers =
                        [
                            new TestAnswer
                            {
                                Answer = new Field { Value = _mockImage, Type = FieldType.Photo },
                                AnswerValue = CorrectAnswer.A
                            },
                            new TestAnswer
                            {
                                Answer = new Field { Value = GetRandomString(answerFactor), Type = FieldType.Text },
                                AnswerValue = CorrectAnswer.B
                            },
                            new TestAnswer
                            {
                                Answer = new Field { Value = GetRandomString(answerFactor), Type = FieldType.Text },
                                AnswerValue = CorrectAnswer.C
                            },
                            new TestAnswer
                            {
                                Answer = new Field { Value = GetRandomString(answerFactor), Type = FieldType.Text },
                                AnswerValue = CorrectAnswer.D
                            }
                        ],
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
                        Answers =
                        [
                            new TestAnswer
                            {
                                Answer = new Field { Value = _mockImage, Type = FieldType.Photo },
                                AnswerValue = CorrectAnswer.A
                            },
                            new TestAnswer
                            {
                                Answer = new Field { Value = GetRandomString(answerFactor), Type = FieldType.Text },
                                AnswerValue = CorrectAnswer.B
                            },
                            new TestAnswer
                            {
                                Answer = new Field { Value = GetRandomString(answerFactor), Type = FieldType.Text },
                                AnswerValue = CorrectAnswer.C
                            },
                            new TestAnswer
                            {
                                Answer = new Field { Value = GetRandomString(answerFactor), Type = FieldType.Text },
                                AnswerValue = CorrectAnswer.D
                            }
                        ],
                        CorrectAnswers = answers
                    });
                    break;
                }
                default:
                    project.Questions.Add(new OpenQuestion
                    {
                        ID = Guid.NewGuid(),
                        QuestionText = GetRandomString(questionFactor),
                        Answer = new Field { Value = string.Join(" ", _lorem), Type = FieldType.Text }
                    });
                    break;
            }
        }
    }

    public void SaveProject(Project project)
    {
        messenger.Send(new SaveFileClickedMessageResponse
        {
            Project = project
        });
    }

    public bool QuestionExists(Project project, Guid id)
    {
        return project.Questions.Any(q => q.ID == id);
    }
    
    private readonly string _mockImage = "data:image/jpg;base64,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";}