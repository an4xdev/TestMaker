﻿namespace TestMaker.Data.Models;

public class TestAnswer
{
    public string Answer { get; set; } = string.Empty;

    //public int AnswerValue { get; set; }
    public CorrectAnswer AnswerValue { get; set; }
}
