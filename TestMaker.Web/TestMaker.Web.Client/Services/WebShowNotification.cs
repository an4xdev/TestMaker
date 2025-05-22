using MudBlazor;
using TestMaker.Data.Services;

namespace TestMaker.Web.Client.Services;

public class WebShowNotification(ISnackbar snackbar) : IShowNotification
{
    public Task ShowNotification(string notificationText)
    {
        snackbar.Add(notificationText);
        return Task.CompletedTask;
    }
}