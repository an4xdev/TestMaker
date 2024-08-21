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
                break;
            case Languages.Polish:
                Button1 = "Kolejne pytanie jednokrotnego wyboru";
                Button2 = "Losowe pytanie jednokrotnego wyboru";
                Button3 = "Kolejne pytanie wielokrotnego wyboru";
                Button4 = "Losowe pytanie wielokrotnego wyboru";
                Button5 = "Kolejne pytanie otwarte";
                Button6 = "Losowe pytanie otwarte";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(language), language, null);
        }
    }
    public string Button1 { get;}
    public string Button2 { get;}
    public string Button3 { get;}
    public string Button4 { get;}
    public string Button5 { get;}
    public string Button6 { get;}
}

public enum Languages
{
    English,
    Polish
}