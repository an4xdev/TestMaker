using CommunityToolkit.Maui.Alerts;
using TestMaker.Data.Services;

namespace TestMaker.Hybrid.Services;

public class DesktopShowNotification : IShowNotification
{
    public async Task ShowNotification(string notificationText)
    {
        await Toast.Make(notificationText).Show();
    }
}