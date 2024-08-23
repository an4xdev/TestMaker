namespace TestMaker.Data.Services.ServiceModels;

public class PageContent
{
    public PageContent(Languages language)
    {
        switch (language)
        {
            case Languages.English:
                Button1 = "Another single-choice question";
                Button2 = "Random single-choice question";
                Button3 = "Another multiple choice question";
                Button4 = "Random multiple choice question";
                Button5 = "Another open question";
                Button6 = "Random open question";
                ShowAnswer = "Show answer";
                break;
            case Languages.Polish:
                Button1 = "Kolejne pytanie jednokrotnego wyboru";
                Button2 = "Losowe pytanie jednokrotnego wyboru";
                Button3 = "Kolejne pytanie wielokrotnego wyboru";
                Button4 = "Losowe pytanie wielokrotnego wyboru";
                Button5 = "Kolejne pytanie otwarte";
                Button6 = "Losowe pytanie otwarte";
                ShowAnswer = "Pokaż odpowiedź";
                break;
            case Languages.None:
                Button1 = string.Empty;
                Button2 = string.Empty;
                Button3 = string.Empty;
                Button4 = string.Empty;
                Button5 = string.Empty;
                Button6 = string.Empty;
                ShowAnswer = string.Empty;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(language), language, null);
        }
    }
    public string Button1 { get; set;}
    public string Button2 { get; set;}
    public string Button3 { get; set;}
    public string Button4 { get; set;}
    public string Button5 { get; set;}
    public string Button6 { get; set;}
    
    public string ShowAnswer { get; set; }
    
    public bool IsEmpty => string.IsNullOrEmpty(Button1) 
                              || string.IsNullOrEmpty(Button2) 
                              || string.IsNullOrEmpty(Button3) 
                              || string.IsNullOrEmpty(Button4) 
                              || string.IsNullOrEmpty(Button5) 
                              || string.IsNullOrEmpty(Button6) 
                              || string.IsNullOrEmpty(ShowAnswer);
    
}

public enum Languages
{
    None,
    English,
    Polish
}