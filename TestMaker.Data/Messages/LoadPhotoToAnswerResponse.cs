namespace TestMaker.Data.Messages;

public class LoadPhotoToAnswerResponse
{
    public Guid AnswerId { get; set; }
    public string PhotoBase64Data { get; set; } = string.Empty;
}