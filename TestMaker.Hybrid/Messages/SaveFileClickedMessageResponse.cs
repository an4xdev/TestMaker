using TestMaker.Data.Models;

namespace TestMaker.Hybrid.Messages;
public class SaveFileClickedMessageResponse
{
    public Project Project { get; set; } = new();
}