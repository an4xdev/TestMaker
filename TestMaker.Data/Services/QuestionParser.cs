using System.Text.RegularExpressions;
using TestMaker.Data.Models;
using TestMaker.Data.Services.ServiceModels;

namespace TestMaker.Data.Services;

public static partial class QuestionParser
{
    /// <summary>
    /// Get project from markdown file.<br />
    /// Loading photos is done by just setting answer type to Photo.<br/>
    /// You should load this photo after separately.
    /// </summary>
    /// <param name="notification">Service for showing notifications</param>
    /// <param name="lines">Lines split by '\n'</param>
    /// <param name="fileName">Clear file name without path or extension</param>
    /// <returns></returns>
    public static async Task<Project?> ParseProjectFromMarkdown(IShowNotification notification, string[] lines,
        string fileName)
    {
        var project = new Project();

        var projectName = false;

        var data = new List<string>();

        var lineCounter = 1;

        foreach (var line in lines)
        {
            lineCounter++;

            if (line.Equals("")) continue;

            if (!projectName)
            {
                if (!line.StartsWith("##"))
                {
                    project.Name = line.Split("#")[1];
                }
                else
                {
                    // project.Name = fileName.Split('.')[0];
                    project.Name = fileName;
                    data.Add(line);
                }

                projectName = true;
                continue;
            }

            if (line.StartsWith("##") && data.Count > 0)
            {
                var parse = await Parse(data);
                if (parse.Question != null)
                {
                    project.Questions.Add(parse.Question);
                    data.Clear();
                }
                else if (parse.Message != null)
                {
                    await notification.ShowNotification(parse.Message);
                }
                else
                {
                    await notification.ShowNotification(
                        $"The question is in an incorrect format. At line: {lineCounter}");
                    return null;
                }
            }

            data.Add(line);
        }

        if (data.Count > 0)
        {
            var parse = await Parse(data);
            if (parse.Question != null)
            {
                project.Questions.Add(parse.Question);
                data.Clear();
            }
            else if (parse.Message != null)
            {
                await notification.ShowNotification(parse.Message);
            }
            else
            {
                await notification.ShowNotification(
                    $"An error occurred while processing the question from the file. At line: {lineCounter}");
                return null;
            }
        }

        return project;
    }

    private static Task<QuestionParseResponse> Parse(List<string> data)
    {
        var boldCounter = data.Count(s => s.Contains("**"));
        switch (boldCounter)
        {
            case 0 when data.Count > 1:
                var questionO = new OpenQuestion
                {
                    ID = Guid.NewGuid(),
                    QuestionText = data[0].Split("##")[1].TrimStart(),
                    Answer = new Field
                    {
                        Value = string.Join("\n", data[1..data.Count]),
                        Type = FieldType.Text
                    }
                };
                return Task.FromResult(new QuestionParseResponse
                {
                    Question = questionO
                });
            case 0 when data.Count < 1:
                return Task.FromResult(new QuestionParseResponse
                {
                    Message =
                        "According to the data read, this should be an open question. Unfortunately, an error was encountered."
                });
            case 1 when data.Count > 1:
            {
                var question = new TestOneQuestion
                {
                    ID = Guid.NewGuid(),
                    QuestionText = data[0].Split("##")[1].TrimStart()
                };

                question.Answers.Clear();
                for (var i = 1; i < data.Count; i++)
                {
                    var answer = new TestAnswer
                    {
                        AnswerValue = (CorrectAnswer)i - 1,
                        Answer = new Field
                        {
                            Value = data[i].Contains("**")
                                ? data[i].Split("- **")[1].Split("**")[0]
                                : data[i].Split("- ")[1],
                            Type = MyRegex().IsMatch(data[i]) ? FieldType.Photo : FieldType.Text
                        }
                    };
                    if (data[i].Contains("**"))
                    {
                        question.CorrectAnswer = (CorrectAnswer)i - 1;
                    }

                    question.Answers.Add(answer);
                }

                return Task.FromResult(new QuestionParseResponse
                {
                    Question = question
                });
            }
            case 1 when data.Count < 1:
                return Task.FromResult(new QuestionParseResponse
                {
                    Message =
                        "According to the data read, this should be an test question with one answer. Unfortunately, an error was encountered."
                });
            case > 1 when data.Count > 1:
            {
                var question = new TestMultiQuestion
                {
                    ID = Guid.NewGuid(),
                    QuestionText = data[0].Split("##")[1].TrimStart()
                };

                question.Answers.Clear();
                for (var i = 1; i < data.Count; i++)
                {
                    var answer = new TestAnswer
                    {
                        Answer = new Field
                        {
                            Value = data[i].Contains("**")
                                ? data[i].Split("- **")[1].Split("**")[0]
                                : data[i].Split("- ")[1],
                            Type = data[i].Contains('!') ? FieldType.Photo : FieldType.Text
                        },
                        AnswerValue = (CorrectAnswer)i - 1
                    };

                    if (data[i].Contains("**"))
                    {
                        question.CorrectAnswers.Add((CorrectAnswer)i - 1);
                    }

                    question.Answers.Add(answer);
                }

                return Task.FromResult(new QuestionParseResponse
                {
                    Question = question
                });
            }
            case > 1 when data.Count < 1:
                return Task.FromResult(new QuestionParseResponse
                {
                    Message =
                        "According to the data read, this should be an test question with multiple answers. Unfortunately, an error was encountered."
                });
        }

        return Task.FromResult(new QuestionParseResponse());
    }

    [GeneratedRegex(@"!\[.*?\]\(.*?\)")]
    private static partial Regex MyRegex();
}