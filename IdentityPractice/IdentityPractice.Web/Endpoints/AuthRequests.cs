// IdentityPractice.Web/Endpoints/AuthRequests.cs

namespace IdentityPractice.Web.Endpoints;

public record LoginRequest(string Email, string Password);
public record RegisterRequest(string Email, string Password);