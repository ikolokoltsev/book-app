using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BookApp.Api.Common;

public class CurrentUser(IHttpContextAccessor accessor) : ICurrentUser
{
    public Guid UserId => Guid.TryParse(accessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub), out var id)
    ? id : throw new InvalidOperationException("No authenticated user on this request.");
}