using TestMaker.Data.Models;

namespace TestMaker.Data.Messages;

public class SaveFileWhenClosingResponse
{
    public Project? Project { get; set; }
}