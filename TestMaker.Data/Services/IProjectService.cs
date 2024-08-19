using TestMaker.Data.Models;
using TestMaker.Data.Services.ServiceModels;

namespace TestMaker.Data.Services;

public interface IProjectService
{
    public void MockData(Project project);
    public void AddQuestion(Project project, Question question);
    public List<Question> GetQuestions(Project project);
    public Question? GetQuestionById(Project project, Guid id);
    public Question? GetRandomQuestion(Project project, QuestionType type);
    public ServiceResponse EditQuestion(Project project, Guid originalId, Question edited);
    public ServiceResponse DeleteQuestion(Project project, Guid id);
    public void SaveProject(Project project);
    public bool QuestionExists(Project project, Guid id);
}
