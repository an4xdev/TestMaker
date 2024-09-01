using TestMaker.Data.Models;
using TestMaker.Data.Services.ServiceModels;

namespace TestMaker.Data.Services;

public static class QuestionParser
{
    public static QuestionParseResponse Parse(List<string> data)
    {
        var boldCounter = data.Count(s => s.Contains("**"));
        switch (boldCounter)
        {
            case 0 when data.Count == 2:
                var questionO = new OpenQuestion
                {
                    ID = Guid.NewGuid(),
                    QuestionText = data[0].Split("##")[1],
                    Answer = data[1]
                };
                return new QuestionParseResponse
                {
                    Question = questionO
                };
            case 0 when data.Count != 2:
                return new QuestionParseResponse
                {
                    Message =
                        "According to the data read, this should be an open question. Unfortunately, an error was encountered."
                };
            case 1 when data.Count == 5:
            {
                var question = new TestOneQuestion
                {
                    ID = Guid.NewGuid(),
                    QuestionText = data[0].Split("##")[1]
                };
                for (var i = 1; i < data.Count; i++)
                {
                    var answer = new TestAnswer
                    {
                        Answer = data[i].Contains("**") ? data[i].Split("- **")[1].Split("**")[0] : data[i].Split("- ")[1],
                        AnswerValue = (CorrectAnswer)i - 1
                    };
                    if (data[i].Contains("**"))
                    {
                        question.CorrectAnswer = (CorrectAnswer)i - 1;
                    }
                    question.Answers.Add(answer);
                }
                return new QuestionParseResponse
                {
                    Question =  question
                };
            }
            case 1 when data.Count != 5:
                return new QuestionParseResponse()
                {
                    Message =
                        "According to the data read, this should be an test question with one answer. Unfortunately, an error was encountered."
                };
            case > 1 and <= 4 when data.Count == 5:
            {
                var question = new TestMultiQuestion
                {
                    ID = Guid.NewGuid(),
                    QuestionText = data[0].Split("##")[1],
                };

                for (var i = 1; i < data.Count; i++)
                {
                    var answer = new TestAnswer
                    {
                        Answer = data[i].Contains("**") ? data[i].Split("- **")[1].Split("**")[0] : data[i].Split("- ")[1],
                        AnswerValue = (CorrectAnswer)i - 1
                    };

                    if (data[i].Contains("**"))
                    {
                        question.CorrectAnswers.Add((CorrectAnswer)i - 1);
                    }
                    
                    question.Answers.Add(answer);
                }
                return new QuestionParseResponse
                {
                    Question = question
                };
            }
            case > 1 and <= 4 when data.Count != 5:
                return new QuestionParseResponse
                {
                    Message =
                        "According to the data read, this should be an test question with multiple answers. Unfortunately, an error was encountered."
                };
        }
        
        return new QuestionParseResponse();
    }
}