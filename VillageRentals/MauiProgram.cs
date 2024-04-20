using Microsoft.Extensions.Logging;
using VillageRentals.Data;
using VillageRentals.Views;

namespace VillageRentals
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<EquipmentListPage>();
            builder.Services.AddTransient<EquipmentPage>();

            builder.Services.AddSingleton<VillageRentalsDatabase>();

            return builder.Build();
        }
    }
}
