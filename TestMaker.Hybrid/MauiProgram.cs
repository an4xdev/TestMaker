using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using MudBlazor.Services;
using TestMaker.Data.Services;
using TestMaker.Hybrid.Services;


namespace TestMaker.Hybrid
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddMudServices();
            builder.Services.AddSingleton<IProjectService, ProjectService>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif
            // https://github.com/dotnet/maui/issues/12223#issuecomment-1358956132
#if WINDOWS
            builder.ConfigureLifecycleEvents(events =>
            {
                events.AddWindows(wndLifeCycleBuilder =>
                {
                    wndLifeCycleBuilder.OnWindowCreated(window =>
                {
                    IntPtr nativeWindowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                    Microsoft.UI.WindowId win32WindowsId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(nativeWindowHandle);
                    Microsoft.UI.Windowing.AppWindow winuiAppWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(win32WindowsId);
                    if (winuiAppWindow.Presenter is Microsoft.UI.Windowing.OverlappedPresenter p)
                    {
                        p.Maximize();
                        //p.IsResizable = false;
                        //p.IsMaximizable = false;
                        //p.IsMinimizable = false;
                    }
                });
                });
            });
#endif
            return builder.Build();
        }
    }
}
