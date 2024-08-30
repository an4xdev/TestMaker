using TestMaker.Data.Models;

namespace TestMaker.Data.Messages;

public class LoadProjectFromFileMessage
{
    public Project Project { get; set; } = new();
}