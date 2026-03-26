// IdentityPractice.Maui/Services/AuthService.cs

using IdentityPractice.Maui.Auth;
using IdentityPractice.Shared.Interfaces;
using IdentityPractice.Shared.Models;
using System.Net.Http.Json;

namespace IdentityPractice.Maui.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private MauiAuthStateProvider? _authStateProvider;
    private readonly MauiProgram.CookieContainerHandler _cookieHandler;

    public void SetAuthStateProvider(MauiAuthStateProvider provider)
    {
        _authStateProvider = provider;
    }

    public AuthService(HttpClient httpClient, MauiProgram.CookieContainerHandler cookieHandler)
    {
        _httpClient = httpClient;
        _cookieHandler = cookieHandler;
    }

    public async Task<AuthResult> LoginAsync(string email, string password)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/login", new
            {
                Email = email,
                Password = password
            });

            if (response.IsSuccessStatusCode)
            {
                var uri = new Uri("https://localhost:7071");
                var cookies = _cookieHandler.CookieContainer.GetCookies(uri);

                foreach (System.Net.Cookie cookie in cookies)
                {
                    if (cookie.Name == ".AspNetCore.Identity.Application")
                        await SecureStorage.SetAsync("identity_cookie", cookie.Value);
                }

                await SecureStorage.SetAsync("is_authenticated", "true");
                _authStateProvider?.NotifyAuthStateChanged();
                return new AuthResult { Success = true };
            }

            return new AuthResult { Success = false, ErrorMessage = "Invalid credentials." };
        }
        catch (Exception ex)
        {
            return new AuthResult { Success = false, ErrorMessage = ex.Message };
        }
    }

    public async Task<AuthResult> RegisterAsync(string email, string password)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/register", new
            {
                Email = email,
                Password = password
            });

            if (response.IsSuccessStatusCode)
                return new AuthResult { Success = true };

            var errors = await response.Content.ReadFromJsonAsync<List<string>>();
            return new AuthResult { Success = false, ErrorMessage = string.Join(", ", errors ?? []) };
        }
        catch (Exception ex)
        {
            return new AuthResult { Success = false, ErrorMessage = ex.Message };
        }
    }

    public async Task LogoutAsync()
    {
        await _httpClient.PostAsync("/api/auth/logout", null);
        SecureStorage.Remove("is_authenticated");
        _authStateProvider?.NotifyAuthStateChanged();
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var val = await SecureStorage.GetAsync("is_authenticated");
        return val == "true";
    }
}