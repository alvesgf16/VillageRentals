using Microsoft.Extensions.Logging;
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
            builder.Services.AddSingleton<CategoryListPage>();
            builder.Services.AddTransient<CategoryPage>();
            builder.Services.AddSingleton<CustomerListPage>();
            builder.Services.AddTransient<CustomerPage>();
            builder.Services.AddSingleton<RentalListPage>();
            builder.Services.AddTransient<RentalPage>();

            return builder.Build();
        }
    }
}
