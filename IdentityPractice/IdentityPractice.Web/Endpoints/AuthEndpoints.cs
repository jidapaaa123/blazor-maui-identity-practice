// IdentityPractice.Web/Endpoints/AuthEndpoints.cs

using IdentityPractice.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;

namespace IdentityPractice.Web.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/api/auth/register", async (
            RegisterRequest req,
            UserManager<ApplicationUser> userManager) =>
        {
            var user = new ApplicationUser { UserName = req.Email, Email = req.Email };
            var result = await userManager.CreateAsync(user, req.Password);

            return result.Succeeded
                ? Results.Ok()
                : Results.BadRequest(result.Errors.Select(e => e.Description));
        });

        app.MapPost("/api/auth/login", async (
            LoginRequest req,
            SignInManager<ApplicationUser> signInManager) =>
        {
            var result = await signInManager.PasswordSignInAsync(
                req.Email, req.Password, isPersistent: false, lockoutOnFailure: false);

            return result.Succeeded
                ? Results.Ok()
                : Results.Unauthorized();
        });

        app.MapPost("/api/auth/logout", async (
            SignInManager<ApplicationUser> signInManager) =>
        {
            await signInManager.SignOutAsync();
            return Results.Ok();
        });
    }
}