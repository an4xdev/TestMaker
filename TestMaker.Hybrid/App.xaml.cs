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
    }
}
