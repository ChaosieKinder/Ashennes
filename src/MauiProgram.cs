using DevExpress.Maui;
using Microsoft.Extensions.Logging;

namespace Ashennes
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseDevExpress()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            
            builder.Services.AddSingleton<Models.UserSettings>();
            builder.Services.AddSingleton<Views.AppShell>();
            builder.Services.AddSingleton<Views.AppShellViewModel>();
            builder.Services.AddSingleton<Views.CastSpellPage.CastSpellPageView>();
            builder.Services.AddSingleton<Views.CastSpellPage.CastSpellPageViewModel>();
            builder.Services.AddSingleton<Views.RulesPage.RulesPageView>();
            builder.Services.AddSingleton<Views.RulesPage.RulesPageViewModel>();
            builder.Services.AddSingleton<Views.SettingsPage.SettingsPageView>();
            builder.Services.AddSingleton<Views.SettingsPage.SettingsPageViewModel>();
            builder.Services.AddScoped<Views.SpellListPage.SpellListPageView>();
            builder.Services.AddScoped<Views.SpellListPage.SpellListPageViewModel>();
            builder.Services.AddScoped<Views.CastSpellConfirmationPage.CastSpellConfirmationPageView>();
            builder.Services.AddScoped<Views.CastSpellConfirmationPage.CastSpellConfirmationPageViewModel>();
            builder.Services.AddScoped<Views.CastSpellResultsPage.CastSpellResultsPageView>();
            builder.Services.AddScoped<Views.CastSpellResultsPage.CastSpellResultsPageViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}