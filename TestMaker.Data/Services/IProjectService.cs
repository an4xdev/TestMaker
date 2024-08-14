﻿using TestMaker.Data.Models;
using TestMaker.Data.Services.ServiceModels;

namespace TestMaker.Data.Services;

public interface IProjectService
{
    public void MockData(Project project);
    public void AddQuestion(Project project, Question question);
    public List<Question> GetQuestions(Project project);
    public Question? GetQuestionByID(Project project, Guid ID);
    public Question? GetRandomQuestion(Project project, QuestionType type);
    public TaskDone EditQuestion(Project project, Guid originalID, Question edited);
    public TaskDone DeleteQuestion(Project project, Question question);
    public TaskDone CreateNew();
    public TaskDone SaveProject(Project project);
}