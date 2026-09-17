using System.IdentityModel.Tokens.Jwt;
using BookApp.Api.Features.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookApp.Api.Features.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController(AuthService authService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(
        RegisterRequest request,
        CancellationToken ct
    )
    {
        var result = await authService.RegisterAsync(request, ct);
        return result is null
            ? Conflict(new ProblemDetails { Title = "We can't register you with this credentials" })
            : Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request, CancellationToken ct)
    {
        var result = await authService.LoginAsync(request, ct);
        return result is null
            ? Unauthorized(new ProblemDetails { Title = "Invalid login or password" })
            : Ok(result);
    }

    [Authorize]
    [HttpGet("me")]
    public ActionResult Me() =>
        Ok(new { userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value });
}
