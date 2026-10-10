using BookStore.Shared.Services;
using BookStore.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Devices;

namespace BookStore;

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
        var apiBaseAddress = DeviceInfo.Platform == DevicePlatform.Android
            ? DeviceInfo.DeviceType == DeviceType.Physical
                ? "http://localhost:5017/"
                : "http://10.0.2.2:5017/"
            : "http://localhost:5017/";
        builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(apiBaseAddress) });
        builder.Services.AddSingleton<BookCatalogClient>();
        builder.Services.AddSingleton<ITokenStore, MauiTokenStore>();
        builder.Services.AddSingleton<StoreApiClient>();
        builder.Services.AddSingleton<ManagerApiClient>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
