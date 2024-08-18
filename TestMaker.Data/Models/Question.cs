using System.Text.Json.Serialization;

namespace TestMaker.Data.Models;
[JsonDerivedType(typeof(TestOneQuestion), nameof(TestOneQuestion))]
[JsonDerivedType(typeof(TestMultiQuestion), nameof(TestMultiQuestion))]
[JsonDerivedType(typeof(OpenQuestion), nameof(OpenQuestion))]
public abstract class Question :ICloneable
{
    public Guid ID { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public abstract object Clone();
}
