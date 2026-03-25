namespace IdentityPractice.Shared.Models;

public class AuthResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public string? Token { get; set; } // used by MAUI side
}