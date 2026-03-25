// IdentityPractice.Maui/Auth/MauiAuthStateProvider.cs

using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using IdentityPractice.Shared.Interfaces;

namespace IdentityPractice.Maui.Auth;

public class MauiAuthStateProvider : AuthenticationStateProvider
{
    private readonly IAuthService _authService;

    public MauiAuthStateProvider(IAuthService authService)
    {
        _authService = authService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var isAuthenticated = await _authService.IsAuthenticatedAsync();

        if (!isAuthenticated)
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        var claims = new[] { new Claim(ClaimTypes.Name, "User") };
        var identity = new ClaimsIdentity(claims, "MauiAuth");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public void NotifyAuthStateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}