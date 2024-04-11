using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;

namespace TeaStoreApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            }).ConfigureMauiHandlers(h =>
            {
#if IOS
                h.AddHandler<Shell, TeaStoreApp.Platforms.iOS.CustomShellRenderer>();

#endif
            }).UseMauiCommunityToolkit();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}