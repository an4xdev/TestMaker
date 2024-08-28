using TestMaker.Data.Models;

namespace TestMaker.Hybrid.Messages;

public class LoadProjectFromFileMessage
{
    public Project Project { get; set; } = new();
}