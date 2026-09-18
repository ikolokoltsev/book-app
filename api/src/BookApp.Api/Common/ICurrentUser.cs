namespace BookApp.Api.Common;

public interface ICurrentUser
{
    Guid UserId { get; }
}