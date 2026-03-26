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

            builder.Services.AddSingleton(cookieHandler); // register it so it can be injected
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

            var app = builder.Build();

            // Restore persisted cookie if it exists
            // Instead of await, use .GetAwaiter().GetResult() or just go synchronous
            var cookieValue = SecureStorage.GetAsync("identity_cookie").GetAwaiter().GetResult();
            if (!string.IsNullOrEmpty(cookieValue))
            {
                cookieHandler.CookieContainer.Add(
                    new Uri("https://localhost:7071"),
                    new System.Net.Cookie(".AspNetCore.Identity.Application", cookieValue)
                );
            }

            return app;
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