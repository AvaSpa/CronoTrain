using CronoTrain.Code.Utils;
using CronoTrain.ViewModels;
using CronoTrain.Views;
using Plugin.Maui.Audio;

namespace CronoTrain;

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
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<MainViewModel>();

        builder.Services.AddTransient<HangTimePage>();
        builder.Services.AddTransient<HangTimeViewModel>();

        builder.Services.AddSingleton(AudioManager.Current);
        builder.Services.AddSingleton(Vibration.Default);

        builder.Services.AddTransient<IAlertManager, AlertManager>();

        return builder.Build();
    }
}
