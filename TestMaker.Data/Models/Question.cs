﻿namespace TestMaker.Data.Models;

public abstract class Question
{
    public Guid ID { get; set; }
    public string QuestionText { get; set; } = string.Empty;
}