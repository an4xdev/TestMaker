using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.Messaging;
using TestMaker.Data.Messages;

namespace TestMaker.Hybrid
{
    public partial class App : Application
    {
        private readonly IMessenger _messenger;
        public App(IFileSaver saver, IMessenger messenger)
        {
            _messenger = messenger;
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage(saver, messenger));
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
