using IdentityPractice.Shared.Models;

namespace IdentityPractice.Shared.Interfaces;

public interface IAuthService
{
    Task<AuthResult> LoginAsync(string email, string password);
    Task<AuthResult> RegisterAsync(string email, string password);
    Task LogoutAsync();
    Task<bool> IsAuthenticatedAsync();
}