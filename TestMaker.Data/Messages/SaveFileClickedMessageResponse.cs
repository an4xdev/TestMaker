using TestMaker.Data.Models;

namespace TestMaker.Data.Messages;
public class SaveFileClickedMessageResponse
{
    public Project Project { get; set; } = new();
}