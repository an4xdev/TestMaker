using TestMaker.Data.Models;
using TestMaker.Data.Services.ServiceModels;

namespace TestMaker.Data.Services;

public interface IProjectService
{
    public void MockData(Project project);
    public void AddQuestion(Project project, Question question);
    public List<Question> GetQuestions(Project project);
    public Question? GetQuestionByID(Project project, Guid ID);
    public Question? GetRandomQuestion(Project project, QuestionType type);
    public ServiceResponse EditQuestion(Project project, Guid originalID, Question edited);
    public ServiceResponse DeleteQuestion(Project project, Guid ID);
    public ServiceResponse CreateNew();
    public ServiceResponse SaveProject(Project project);
}
