using System.ComponentModel.DataAnnotations;

namespace BookApp.Api.Features.Auth;

public record RegisterRequest(
    [Required, MinLength(3), MaxLength(50)] string Username,
    [Required, MinLength(8)] string Password
);

public record LoginRequest([Required] string Username, [Required] string Password);

public record AuthResponse(string Token, DateTime ExpiresAt);