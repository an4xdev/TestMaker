using CommunityToolkit.Mvvm.Messaging;
using MudBlazor;
using MudBlazor.Services;
using TestMaker.Data.Services;
using TestMaker.Web.Client.Pages;
using TestMaker.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddSingleton<IMessenger, WeakReferenceMessenger>();
builder.Services.AddSingleton<IProjectService, ProjectService>();

builder.Services.AddMudServices(
//     config =>
// {
//     config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopRight;
//     config.SnackbarConfiguration.PreventDuplicates = false;
//     config.SnackbarConfiguration.NewestOnTop = true;
//     config.SnackbarConfiguration.ShowCloseIcon = true;
//     config.SnackbarConfiguration.VisibleStateDuration = 10000;
//     config.SnackbarConfiguration.HideTransitionDuration = 500;
//     config.SnackbarConfiguration.ShowTransitionDuration = 500;
//     config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
// }
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(TestMaker.Web.Client._Imports).Assembly)
    .AddAdditionalAssemblies(typeof(TestMaker.Shared.Pages.Application).Assembly);

app.Run();
