using CommunityToolkit.Mvvm.Messaging;
using TestMaker.Data.Delegates;
using TestMaker.Data.Exceptions;
using TestMaker.Data.Messages;
using TestMaker.Data.Models;
using TestMaker.Data.Services.ServiceModels;

namespace TestMaker.Data.Services;

public class ProjectService(IMessenger messenger) : IProjectService
{
    public void DeleteQuestion(Project project, Guid id)
    {
        var isSuccess = project.Questions.RemoveAll(x => x.ID == id) == 1;
        if (!isSuccess)
        {
            throw new QuestionNotFoundException("Cannot delete question that's not available");
        }

        StatsUpdate?.Invoke(GetQuestionCount(project));
    }

    public Question GetQuestionById(Project project, Guid id)
    {
        var question = project.Questions.Find(q => q.ID == id);

        if (question == null)
        {
            throw new QuestionNotFoundException("Cannot get question that's not available");
        }

        return question;
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

    public void MockData(Project project)
    {
        const int questionFactor = 25;
        const int answerFactor = 16;

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
                                Answer = new Field { Value = GetRandomString(answerFactor), Type = FieldType.Text },
                                AnswerValue = CorrectAnswer.A
                                // Answer = new Field { Value = MockImage, Type = FieldType.Photo },
                                // AnswerValue = CorrectAnswer.A
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
                case < n / 3 * 2:
                {
                    var rand = random.Next(1, 4);

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
                                // Answer = new Field { Value = MockImage, Type = FieldType.Photo },
                                // AnswerValue = CorrectAnswer.A
                                Answer = new Field { Value = GetRandomString(answerFactor), Type = FieldType.Text },
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
                    var start = random.Next(0, Resources.Lorem.Length / 2);
                    var end = random.Next(Resources.Lorem.Length / 2, Resources.Lorem.Length);
                    project.Questions.Add(new OpenQuestion
                    {
                        ID = Guid.NewGuid(),
                        QuestionText = GetRandomString(questionFactor),
                        Answer = new Field
                            { Value = string.Join(" ", Resources.Lorem[start..end]), Type = FieldType.Text }
                    });
                    break;
            }
        }

        StatsUpdate?.Invoke(GetQuestionCount(project));
    }

    public void SaveProject(Project project)
    {
        messenger.Send(new SaveFileClickedMessageResponse
        {
            Project = project
        });
    }

    public void AddOrEditQuestion(Project project, Question edited, Guid originalId)
    {
        if (QuestionExists(project, originalId))
        {
            EditQuestion(project, originalId, edited);
        }
        else
        {
            AddQuestion(project, edited);
        }
    }

    public event StatsUpdate? StatsUpdate;

    public void UpdateStatsOnLoad(Project project)
    {
        StatsUpdate?.Invoke(GetQuestionCount(project));
    }

    public void UpdateQuestionPhotoData(Project project, Guid testAnswerId, string data)
    {
        var answer = project.Questions.OfType<TestQuestion>().SelectMany(tq => tq.Answers)
            .FirstOrDefault(ta => ta.Id == testAnswerId);
        if (answer == null) throw new TestAnswerNotFoundException($"TestAnswer not found got ID: {testAnswerId}");

        answer.Answer.Value = data;
    }

    private void AddQuestion(Project project, Question question)
    {
        project.Questions.Add(question);
        StatsUpdate?.Invoke(GetQuestionCount(project));
    }

    private static void EditQuestion(Project project, Guid originalId, Question edited)
    {
        var index = project.Questions.FindIndex(q => q.ID == originalId);

        if (index == -1) throw new QuestionNotFoundException("Cannot edit question that's not available");

        project.Questions[index] = edited;
    }

    private string GetRandomString(int factor)
    {
        Random random = new();
        List<string> list = [];

        var count = random.Next(Resources.Lorem.Length / factor, Resources.Lorem.Length / factor * 3);

        for (var i = 0; i < count; i++) list.Add(Resources.Lorem[random.Next(Resources.Lorem.Length)]);

        return string.Join(" ", list);
    }

    private static bool QuestionExists(Project project, Guid id)
    {
        return project.Questions.Any(q => q.ID == id);
    }

    private static QuestionCount GetQuestionCount(Project project)
    {
        var testOneQuestionCount = project.Questions.OfType<TestOneQuestion>().Count();
        var testMultiQuestionCount = project.Questions.OfType<TestMultiQuestion>().Count();
        var openQuestionCount = project.Questions.OfType<OpenQuestion>().Count();

        return new QuestionCount(testOneQuestionCount, testMultiQuestionCount, openQuestionCount);
    }
}