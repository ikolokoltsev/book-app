
using BookApp.Domain;
using BookApp.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookApp.Api.Features.Auth;

public class AuthService(AppDbContext db, TokenService tokenService)
{
    private readonly PasswordHasher<User> _hasher = new();

    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request, CancellationToken ct)
    {
        var takenUser = await db.Users.AnyAsync(u => u.Username == request.Username, ct);
        if (takenUser) return null;

        var user = new User { Username = request.Username, PasswordHash = string.Empty };
        user.PasswordHash = _hasher.HashPassword(user, request.Password);

        db.Users.Add(user);
        await db.SaveChangesAsync(ct);

        return tokenService.CreateToken(user);
    }
    public async Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        var user = await db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == request.Username, ct);

        if (user is null) return null;

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed) return null;

        return tokenService.CreateToken(user);
    }
}