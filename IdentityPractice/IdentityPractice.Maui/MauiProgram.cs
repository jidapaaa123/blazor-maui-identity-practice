using IdentityPractice.Maui.Auth;
using IdentityPractice.Maui.Services;
using IdentityPractice.Shared.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

namespace IdentityPractice.Maui
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
                });

            builder.Services.AddMauiBlazorWebView();

            // Create a shared cookie handler
            var cookieHandler = new CookieContainerHandler();

            // Register ONE shared HttpClient with cookies
            builder.Services.AddSingleton(new HttpClient(cookieHandler)
            {
                BaseAddress = new Uri("https://localhost:7071")
            });

            // Register services — both get the same HttpClient injected
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IVacationService, VacationService>();

            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<MauiAuthStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
                sp.GetRequiredService<MauiAuthStateProvider>());

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        public class CookieContainerHandler : HttpClientHandler
        {
            public CookieContainerHandler()
            {
                UseCookies = true;
                CookieContainer = new System.Net.CookieContainer();
            }
        }
    }
}