using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using BookApp.Domain;

using Microsoft.IdentityModel.Tokens;

namespace BookApp.Api.Features.Auth;

public class TokenService(IConfiguration configuration)
{
    public AuthResponse CreateToken(User user)
    {
        var key =
            configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("Jwt:Key is not configured.");
        var issuer = configuration["Jwt:Issuer"]!;
        var audience = configuration["Jwt:Audience"]!;
        var expires = DateTime.UtcNow.AddHours(configuration.GetValue("Jwt:ExpiryHours", 1));

        Claim[] claims =
        [
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.Username),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        ];

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: expires,
            signingCredentials: credentials
        );

        return new AuthResponse(new JwtSecurityTokenHandler().WriteToken(token), expires);
    }
}