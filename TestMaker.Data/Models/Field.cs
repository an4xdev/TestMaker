namespace TestMaker.Data.Models;

public class Field : ICloneable
{
    public string Value { get; set; } = string.Empty;

    public FieldType Type { get; set; }

    public object Clone()
    {
        return new Field
        {
            Value = Value,
            Type = Type,
        };
    }
}