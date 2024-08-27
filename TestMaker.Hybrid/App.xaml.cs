using CommunityToolkit.Maui.Storage;

namespace TestMaker.Hybrid
{
    public partial class App : Application
    {
        public App(IFileSaver saver)
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage(saver));
        }
        
        // https://github.com/dotnet/maui/issues/11263#issuecomment-1384487707
        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);
            window.Title = $"Test Maker by Michał Żuk \u00a9 {DateTime.Now.Year}";
            return window;
        }
    }
}
