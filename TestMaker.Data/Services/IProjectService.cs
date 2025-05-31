using TestMaker.Data.Delegates;
using TestMaker.Data.Exceptions;
using TestMaker.Data.Models;
using TestMaker.Data.Services.ServiceModels;

namespace TestMaker.Data.Services;

public interface IProjectService
{
    public void MockData(Project project);
    public List<Question> GetQuestions(Project project);

    /// <summary>
    /// Get question by id
    /// </summary>
    /// <param name="project">Project object</param>
    /// <param name="id">ID of question</param>
    /// <returns>Question by specified ID</returns>
    /// <exception cref="QuestionNotFoundException">When question isn't in project</exception>
    public Question? GetQuestionById(Project project, Guid id);

    public Question? GetRandomQuestion(Project project, QuestionType type);

    /// <summary>
    /// Delete question by id
    /// </summary>
    /// <param name="project">Project object</param>
    /// <param name="id">Question id</param>
    /// <exception cref="QuestionNotFoundException">When question isn't in project</exception>
    public void DeleteQuestion(Project project, Guid id);

    public void SaveProject(Project project);
    public void AddOrEditQuestion(Project project, Question edited, Guid originalId);
    public event StatsUpdate StatsUpdate;
    public void UpdateStatsOnLoad(Project project);

    /// <summary>
    /// Update test answer data with photo data after loading from markdown
    /// </summary>
    /// <param name="project">Project object</param>
    /// <param name="testAnswerId">Test answer ID</param>
    /// <param name="data">photo data</param>
    /// <exception cref="TestAnswerNotFoundException">When test answer isn't in project</exception>
    public void UpdateQuestionPhotoData(Project project, Guid testAnswerId, string data);
}