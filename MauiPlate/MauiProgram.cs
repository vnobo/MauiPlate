using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Toolkit.Hosting;

namespace MauiPlate;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
#if DEBUG
            .UseMauiCommunityToolkit(static options => { options.SetShouldEnableSnackbarOnWindows(true); })
#else
            .UseMauiCommunityToolkit(static options =>
								{
									options.SetShouldEnableSnackbarOnWindows(true);
									options.SetShouldSuppressExceptionsInConverters(true);
									options.SetShouldSuppressExceptionsInBehaviors(true);
									options.SetShouldSuppressExceptionsInAnimations(true);
								})
#endif
            .ConfigureSyncfusionToolkit()
            .ConfigureMauiHandlers(handlers => { })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("LXGWWenKai-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("LXGWWenKai-Bold.ttf", "OpenSansSemibold");
                fonts.AddFont("LXGWWenKai-Bold.ttf", "SegoeSemibold");
                fonts.AddFont("FluentSystemIcons-Regular.ttf", FluentUI.FontFamily);
            });
#if DEBUG
        builder.Logging.AddDebug();
        builder.Services.AddLogging(configure => configure.AddDebug());
#endif

        builder.Services.AddSingleton<ProjectRepository>();
        builder.Services.AddSingleton<TaskRepository>();
        builder.Services.AddSingleton<CategoryRepository>();
        builder.Services.AddSingleton<TagRepository>();
        builder.Services.AddSingleton<SeedDataService>();
        builder.Services.AddSingleton<ModalErrorHandler>();
        builder.Services.AddSingleton<MainPageModel>();
        builder.Services.AddSingleton<ProjectListPageModel>();
        builder.Services.AddSingleton<ManageMetaPageModel>();

        builder.Services.AddTransientWithShellRoute<ProjectDetailPage, ProjectDetailPageModel>("project");
        builder.Services.AddTransientWithShellRoute<TaskDetailPage, TaskDetailPageModel>("task");
        return builder.Build();
    }
}